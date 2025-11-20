# Number Word Analyzer API

An ASP.NET Core Web API that analyzes concatenated text strings and counts occurrences of number words (one, two, three, four, five, six, seven, eight, nine). The API handles overlapping matches, meaning if "one" appears within another word or overlaps with other characters, all occurrences are counted.

## Features

- **Number Word Detection**: Identifies and counts all occurrences of number words (1-9) in concatenated text
- **Overlapping Match Support**: Uses advanced regex patterns to detect overlapping occurrences
- **Rate Limiting**: Implements IP-based rate limiting (10 requests per 10 seconds per IP)
- **API Documentation**: Integrated Swagger/OpenAPI documentation (available in all environments at `/swagger`)
- **Input Validation**: Validates input text (1-1000 characters)
- **Global Exception Handling**: Comprehensive error handling with descriptive error messages
- **Unit Tests**: Full test coverage with xUnit and Moq
- **Docker Support**: Containerized deployment ready
- **Cloud Deployment Ready**: Configured for Render.com, Railway, Heroku, and other cloud platforms

## Technologies Used

- .NET 9.0
- ASP.NET Core Web API
- Swagger/OpenApi
- xUnit (Testing)
- Moq (Mocking)
- Docker

## Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or higher
- Docker (optional, for containerized deployment)

## Getting Started

### Clone the Repository

```bash
git clone <repository-url>
cd NumberWordAnalyzer
```

### Running Locally

1. **Restore dependencies**
   ```bash
   dotnet restore
   ```

2. **Build the project**
   ```bash
   dotnet build
   ```

3. **Run the application**
   ```bash
   dotnet run --project NumberWordAnalyzer/NumberWordAnalyzer.csproj
   ```

4. **Access the API** (Development uses ports 5000/5001)
   - API: `http://localhost:5000` or `https://localhost:5001`
   - Swagger UI: `http://localhost:5000/swagger` or `https://localhost:5001/swagger`

   > **Note**: When deployed to cloud or running in Docker, the app uses port 8080 (or the `PORT` environment variable)

### Running with Docker

1. **Build the Docker image**
   ```bash
   docker build -t numberwordanalyzer .
   ```

2. **Run the container**
   ```bash
   docker run -p 8080:8080 numberwordanalyzer
   ```

3. **Access the API**
   - API: `http://localhost:8080`
   - Swagger UI: `http://localhost:8080/swagger`

## Cloud Deployment

The application is configured for cloud deployment platforms like Render.com, Railway, Heroku, etc.

### Key Features

- **Dynamic Port Configuration**: Reads `PORT` environment variable (defaults to 8080)
- **Swagger in Production**: API documentation available in all environments at `/swagger`
- **SSL/TLS Handling**: HTTPS redirection disabled in production (cloud providers handle SSL termination)
- **IP-based Rate Limiting**: Works across distributed deployments

### Deployment Steps

1. **Set Environment Variables** (if required by your platform):
   ```bash
   PORT=8080  # Usually auto-configured by the platform
   ```

2. **Deploy using Docker**:
   - Most cloud platforms support Dockerfile-based deployments
   - The app will automatically use the `PORT` environment variable

3. **Deploy from Git**:
   - Connect your repository to the deployment platform
   - The platform will detect the .NET project and build automatically

4. **Access your deployed API**:
   - API: `https://your-app.platform.com`
   - Swagger UI: `https://your-app.platform.com/swagger`

### Supported Platforms

- **Render.com**: Fully configured and tested
- **Railway**: Compatible with PORT environment variable
- **Heroku**: Compatible (requires Dockerfile deployment)
- **Azure App Service**: Compatible
- **AWS Elastic Beanstalk**: Compatible

## API Documentation

### Endpoint

**POST** `/api/NumberWordAnalyzer`

Analyzes a concatenated word and counts occurrences of number words.

### Request Body

```json
{
  "inputText": "onewgftwogewgonedawdtwothreedfrfourzone"
}
```

### Response

```json
{
  "one": 3,
  "two": 2,
  "three": 1,
  "four": 1,
  "five": 0,
  "six": 0,
  "seven": 0,
  "eight": 0,
  "nine": 0
}
```

### Status Codes

- `200 OK` - Successfully analyzed the text
- `400 Bad Request` - Invalid input (empty, null, or exceeds 1000 characters)
- `429 Too Many Requests` - Rate limit exceeded
- `500 Internal Server Error` - Unexpected error occurred

### Example using cURL

```bash
curl -X POST "http://localhost:5000/api/NumberWordAnalyzer" \
  -H "Content-Type: application/json" \
  -d '{"inputText":"onewgftwogewgonedawdtwothreedfrfourzone"}'
```

### Example using PowerShell

```powershell
Invoke-RestMethod -Uri "http://localhost:5000/api/NumberWordAnalyzer" `
  -Method Post `
  -ContentType "application/json" `
  -Body '{"inputText":"onewgftwogewgonedawdtwothreedfrfourzone"}'
```

## Rate Limiting

The API implements rate limiting to prevent abuse:
- **Limit**: 10 requests per 10 seconds per IP address
- **Response**: When exceeded, returns HTTP 429 with retry-after information

```json
{
  "error": "Too many requests. Slow down!",
  "retryAfter": "10"
}
```

## Running Tests

Run all unit tests:

```bash
dotnet test
```

Run tests with detailed output:

```bash
dotnet test --verbosity detailed
```

Generate test coverage report:

```bash
dotnet test /p:CollectCoverage=true
```

## Project Structure

```
NumberWordAnalyzer/
├── NumberWordAnalyzer/              # Main API project
│   ├── Application/                 # DTOs and application models
│   │   └── AnalyzeNumberWordDto.cs
│   ├── Controllers/                 # API controllers
│   │   └── NumberWordAnalyzerController.cs
│   ├── Domain/                      # Domain models
│   │   └── NumberWords.cs
│   ├── Services/                    # Business logic services
│   │   ├── IAnalyzerService.cs
│   │   └── AnalyzerService.cs
│   └── Program.cs                   # Application entry point
├── NumberWordAnalyzer.Tests/        # Unit tests
│   ├── Controllers/
│   │   └── NumberWordAnalyzerControllerTests.cs
│   └── Services/
│       └── AnalyzerServiceTests.cs
├── Dockerfile                       # Docker configuration
├── .dockerignore                    # Docker ignore file
└── NumberWordAnalyzer.sln          # Solution file
```

## How It Works

The analyzer uses a lookahead regex pattern to detect overlapping matches:

1. **Input**: Receives concatenated text (e.g., `"onetwothree"`)
2. **Pattern Matching**: Uses regex with positive lookahead `(?=(word))` to find all occurrences
3. **Counting**: Counts matches for each number word (one through nine)
4. **Response**: Returns a dictionary with counts for all number words

### Example Analysis

**Input**: `"onewgftwogewgonedawdtwothreedfrfourzone"`

**Breakdown**:
- "one" appears at positions: 0, 14, 37 → Count: 3
- "two" appears at positions: 6, 18 → Count: 2
- "three" appears at position: 24 → Count: 1
- "four" appears at position: 33 → Count: 1
- "zone" at the end contains "one" → Already counted

## Configuration

### Port Configuration

The application automatically configures ports based on the environment:

**Local Development** (`launchSettings.json`):
```bash
export ASPNETCORE_URLS="http://localhost:5000;https://localhost:5001"
```

**Cloud Deployment** (reads from environment variable):
```bash
export PORT=8080  # Or any port required by your cloud provider
```

**Code Reference**: See `Program.cs:8-13` for the PORT configuration logic.

### Rate Limit Settings

Edit `Program.cs:43-70` to modify rate limiting:

```csharp
PermitLimit = 10,                          // Requests per window
Window = TimeSpan.FromSeconds(10),         // Time window
```

### Environment-Specific Behavior

- **Development**: HTTPS redirection enabled, uses ports 5000/5001
- **Production/Cloud**: HTTPS redirection disabled (handled by cloud provider), uses PORT environment variable (default: 8080)
- **Swagger**: Enabled in all environments at `/swagger`

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## Author

**Nyaladzi Tjibha**
- Email: tjibhanyaladzi@gmail.com

## License

This project is available for use under standard software licensing terms.

## Acknowledgments

- Built with ASP.NET Core 9.0
- Swagger/OpenAPI for API documentation
- xUnit and Moq for testing framework
