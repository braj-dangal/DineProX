# DineProX Docker Setup

This document provides instructions for building and running the DineProX application using Docker.

## Prerequisites

- Docker Desktop (Windows/Mac) or Docker Engine (Linux)
- Docker Compose
- At least 4GB of available RAM
- 10GB of available disk space

## Quick Start

### Development Environment

1. **Build and run using PowerShell script:**
   ```powershell
   .\build-docker.ps1
   ```

2. **Or manually:**
   ```bash
   # Build the image
   docker build -t dineprox:latest .
   
   # Run with docker-compose
   docker-compose up -d
   ```

### Production Environment

1. **Set environment variables:**
   ```bash
   # Set a strong database password
   $env:DB_PASSWORD="YourStrongPassword123!"
   ```

2. **Build and run:**
   ```powershell
   .\build-docker.ps1 -Environment production
   ```

## Services

The Docker setup includes the following services:

| Service | Port | Description |
|---------|------|-------------|
| **dineprox-api** | 5000 | Main application API |
| **postgres** | 5432 | PostgreSQL database |
| **redis** | 6379 | Redis cache |

## Configuration

### Environment Variables

| Variable | Default | Description |
|----------|---------|-------------|
| `POSTGRES_PASSWORD` | `dineprox_password` | PostgreSQL password |
| `POSTGRES_USER` | `dineprox_user` | PostgreSQL username |
| `POSTGRES_DB` | `dineprox` | PostgreSQL database name |
| `ASPNETCORE_ENVIRONMENT` | `Production` | .NET environment |

### Database Connection

The application automatically connects to the PostgreSQL instance using:
```
Host=postgres;Database=dineprox;Username=dineprox_user;Password=dineprox_password
```

## Docker Files

| File | Purpose |
|------|---------|
| `Dockerfile` | Development build |
| `Dockerfile.prod` | Production-optimized build |
| `docker-compose.yml` | Development services |
| `docker-compose.prod.yml` | Production services |
| `.dockerignore` | Excluded files from build |

## Useful Commands

### View Logs
```bash
# All services
docker-compose logs -f

# Specific service
docker-compose logs -f dineprox-api
```

### Stop Services
```bash
docker-compose down
```

### Rebuild and Restart
```bash
docker-compose up --build -d
```

### Access Database
```bash
# Connect to SQL Server
docker exec -it dineprox_sqlserver_1 /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P DineProX@123
```

### Clean Up
```bash
# Remove containers and volumes
docker-compose down -v

# Remove images
docker rmi dineprox:latest
```

## Health Checks

All services include health checks:

- **API**: HTTP health check on `/health`
- **Database**: SQL query check
- **Redis**: PING command

## Security Features

### Production Build
- Non-root user execution
- Alpine Linux base image
- Minimal attack surface
- Resource limits
- Health checks

### Development Build
- Full debugging capabilities
- Hot reload support
- Development tools included

## Troubleshooting

### Common Issues

1. **Port conflicts:**
   ```bash
   # Check what's using the ports
   netstat -ano | findstr :5000
   netstat -ano | findstr :1433
   ```

2. **Database connection issues:**
   ```bash
   # Check database logs
   docker-compose logs sqlserver
   ```

3. **Application startup issues:**
   ```bash
   # Check application logs
   docker-compose logs dineprox-api
   ```

### Performance Optimization

1. **Increase memory limits:**
   ```yaml
   deploy:
     resources:
       limits:
         memory: 2G
   ```

2. **Use production build:**
   ```bash
   docker build -f Dockerfile.prod -t dineprox:prod .
   ```

## Monitoring

### Container Status
```bash
docker-compose ps
```

### Resource Usage
```bash
docker stats
```

### Health Check Status
```bash
docker inspect --format='{{.State.Health.Status}}' dineprox_dineprox-api_1
```

## Backup and Restore

### Database Backup
```bash
# Create backup
docker exec dineprox_sqlserver_1 /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P DineProX@123 -Q "BACKUP DATABASE DineProX TO DISK = '/var/opt/mssql/backup/DineProX.bak'"

# Copy from container
docker cp dineprox_sqlserver_1:/var/opt/mssql/backup/DineProX.bak ./backup/
```

### Database Restore
```bash
# Copy backup to container
docker cp ./backup/DineProX.bak dineprox_sqlserver_1:/var/opt/mssql/backup/

# Restore database
docker exec dineprox_sqlserver_1 /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P DineProX@123 -Q "RESTORE DATABASE DineProX FROM DISK = '/var/opt/mssql/backup/DineProX.bak'"
```

## Support

For issues related to:
- **Docker setup**: Check this documentation
- **Application issues**: Check application logs
- **Database issues**: Check SQL Server logs
- **Performance**: Monitor resource usage 