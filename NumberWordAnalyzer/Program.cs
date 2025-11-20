using System.Reflection;
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

    // Include XML comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath);
});

// Dependecy Injection
builder.Services.AddScoped<IAnalyzerService, AnalyzerService>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
