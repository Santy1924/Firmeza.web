# Firmeza.Tests - Dockerfile for running tests

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copy solution and project files
COPY Firmeza.web.sln ./
COPY Firmezaa.web/Firmezaa.web.csproj Firmezaa.web/
COPY Web.Api/Web.Api.csproj Web.Api/
COPY Firmeza.Tests/Firmeza.Tests.csproj Firmeza.Tests/

# Restore dependencies
RUN dotnet restore Firmeza.Tests/Firmeza.Tests.csproj

# Copy all source files
COPY . .

# Build the test project
WORKDIR /src/Firmeza.Tests
RUN dotnet build -c Release --no-restore

# Run tests
ENTRYPOINT ["dotnet", "test", "--no-build", "-c", "Release", "--logger:trx"]
