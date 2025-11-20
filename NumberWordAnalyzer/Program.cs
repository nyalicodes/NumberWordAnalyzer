using System.Reflection;
using System.Threading.RateLimiting;
using Microsoft.OpenApi.Models;
using NumberWordAnalyzer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Number Word Analyzer API",
        Description = "An ASP.NET Core Web API for analyzing concatenated words and counting occurrences of number words (one, two, three, four, five, six, seven, eight, nine)",
        Contact = new OpenApiContact
        {
            Name = "Nyaladzi Tjibha",
            Email = "tjibhanyaladzi@gmail.com"
        }
    });

    // Include XML comments if available
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath);
});

// Dependecy Injection
builder.Services.AddScoped<IAnalyzerService, AnalyzerService>();

// Rate limiting
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 10,       // Allow 10 requests per IP
                Window = TimeSpan.FromSeconds(10),  // per 10 seconds
                QueueLimit = 0,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst
            }
        )
    );

    // Return custom message when rate limit is exceeded
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 429;
        await context.HttpContext.Response.WriteAsJsonAsync(new
        {
            error = "Too many requests. Slow down!",
            retryAfter = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retry)
                ? retry.ToString()
                : "unknown"
        });
    };
});


var app = builder.Build();

// Global Exception Handling
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        var errorFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();

        var errorResponse = new
        {
            error = "An unexpected error occurred.",
            details = errorFeature?.Error.Message
        };

        await context.Response.WriteAsJsonAsync(errorResponse);
    });
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRateLimiter();

app.UseAuthorization();

// Root endpoint
app.MapGet("/", () =>
    "Welcome to NumberWordAnalyzer API. Visit /swagger/index.html to explore and test endpoints."
);

app.MapControllers();

// Render.com port handling
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080"; // Use Render-assigned port
app.Urls.Add($"http://*:{port}");

app.Run();
