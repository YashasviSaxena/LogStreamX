# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything
COPY . .

# Go to correct project folder explicitly
WORKDIR /src/LogStreamX.API

# Restore ONLY API project
RUN dotnet restore LogStreamX.API.csproj

# Build + publish
RUN dotnet publish LogStreamX.API.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "LogStreamX.API.dll"]