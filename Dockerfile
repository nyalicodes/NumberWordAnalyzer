# Stage 1: Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Stage 2: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy solution and project files
COPY ["NumberWordAnalyzer.sln", "."]
COPY ["NumberWordAnalyzer/NumberWordAnalyzer.csproj", "NumberWordAnalyzer/"]
COPY ["NumberWordAnalyzer.Tests/NumberWordAnalyzer.Tests.csproj", "NumberWordAnalyzer.Tests/"]

# Restore dependencies
RUN dotnet restore "NumberWordAnalyzer.sln"

# Copy all source files
COPY . .

# Build the project
WORKDIR "/src/NumberWordAnalyzer"
RUN dotnet build "NumberWordAnalyzer.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage 3: Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "NumberWordAnalyzer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 4: Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NumberWordAnalyzer.dll"]
