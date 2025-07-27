# Multi-stage build for DineProX ABP Framework application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["DineProX.sln", "./"]
COPY ["src/DineProX.HttpApi.Host/DineProX.HttpApi.Host.csproj", "src/DineProX.HttpApi.Host/"]
COPY ["src/DineProX.Application/DineProX.Application.csproj", "src/DineProX.Application/"]
COPY ["src/DineProX.Application.Contracts/DineProX.Application.Contracts.csproj", "src/DineProX.Application.Contracts/"]
COPY ["src/DineProX.Domain/DineProX.Domain.csproj", "src/DineProX.Domain/"]
COPY ["src/DineProX.Domain.Shared/DineProX.Domain.Shared.csproj", "src/DineProX.Domain.Shared/"]
COPY ["src/DineProX.EntityFrameworkCore/DineProX.EntityFrameworkCore.csproj", "src/DineProX.EntityFrameworkCore/"]
COPY ["src/DineProX.HttpApi/DineProX.HttpApi.csproj", "src/DineProX.HttpApi/"]
COPY ["src/DineProX.HttpApi.Client/DineProX.HttpApi.Client.csproj", "src/DineProX.HttpApi.Client/"]
COPY ["src/DineProX.DbMigrator/DineProX.DbMigrator.csproj", "src/DineProX.DbMigrator/"]

# Restore dependencies
RUN dotnet restore "DineProX.sln"

# Copy source code
COPY . .

# Build the application
WORKDIR "/src/src/DineProX.HttpApi.Host"
RUN dotnet build "DineProX.HttpApi.Host.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "DineProX.HttpApi.Host.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80;https://+:443

# Create non-root user for security
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

ENTRYPOINT ["dotnet", "DineProX.HttpApi.Host.dll"] 