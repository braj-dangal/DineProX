# DineProX Docker Build Script
# This script builds and runs the DineProX application using Docker

param(
    [string]$Environment = "development",
    [string]$ImageName = "dineprox",
    [string]$Tag = "latest"
)

Write-Host "🚀 Building DineProX Docker Image..." -ForegroundColor Green

# Set environment-specific variables
if ($Environment -eq "production") {
    $Dockerfile = "Dockerfile.prod"
    $ComposeFile = "docker-compose.prod.yml"
    Write-Host "📦 Using production configuration" -ForegroundColor Yellow
} elseif ($Environment -eq "render") {
    $Dockerfile = "Dockerfile.prod"
    $ComposeFile = "docker-compose.render.yml"
    Write-Host "☁️ Using Render cloud configuration" -ForegroundColor Yellow
} else {
    $Dockerfile = "Dockerfile"
    $ComposeFile = "docker-compose.yml"
    Write-Host "🔧 Using development configuration" -ForegroundColor Yellow
}

# Build the Docker image
Write-Host "🔨 Building Docker image: $ImageName:$Tag" -ForegroundColor Cyan
docker build -f $Dockerfile -t $ImageName`:$Tag .

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Docker build failed!" -ForegroundColor Red
    exit 1
}

Write-Host "✅ Docker image built successfully!" -ForegroundColor Green

# Run with docker-compose
Write-Host "🚀 Starting services with docker-compose..." -ForegroundColor Cyan
docker-compose -f $ComposeFile up -d

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Docker-compose failed!" -ForegroundColor Red
    exit 1
}

Write-Host "✅ Services started successfully!" -ForegroundColor Green
Write-Host ""
Write-Host "📋 Service Information:" -ForegroundColor Yellow
Write-Host "   🌐 API: http://localhost:5000" -ForegroundColor White
Write-Host "   🗄️  Database: localhost:1433" -ForegroundColor White
Write-Host "   🔴 Redis: localhost:6379" -ForegroundColor White
Write-Host ""
Write-Host "📝 Useful Commands:" -ForegroundColor Yellow
Write-Host "   View logs: docker-compose -f $ComposeFile logs -f" -ForegroundColor White
Write-Host "   Stop services: docker-compose -f $ComposeFile down" -ForegroundColor White
Write-Host "   Rebuild: docker-compose -f $ComposeFile up --build" -ForegroundColor White 