# Deploying DineProX to Render.com

This guide will help you deploy your DineProX ABP Framework application to [Render.com](https://render.com/) using PostgreSQL and Redis.

## Prerequisites

- A Render.com account
- Your DineProX code pushed to a Git repository (GitHub, GitLab, etc.)
- PostgreSQL and Redis services will be provisioned automatically

## Quick Deployment

### Option 1: Using Render Blueprint (Recommended)

1. **Fork or clone your repository** to ensure it's accessible
2. **Go to Render Dashboard** and click "New +"
3. **Select "Blueprint"** from the options
4. **Connect your repository** and select the branch
5. **Render will automatically detect** the `render.yaml` file
6. **Review the services** that will be created:
   - PostgreSQL Database
   - Redis Cache
   - Web Service (DineProX API)
7. **Click "Apply"** to deploy

### Option 2: Manual Setup

#### Step 1: Create PostgreSQL Database

1. Go to Render Dashboard → "New +" → "PostgreSQL"
2. Configure:
   - **Name**: `dineprox-postgres`
   - **Database**: `dineprox`
   - **User**: `dineprox_user`
   - **Plan**: Choose based on your needs (Free tier available)
3. Click "Create Database"
4. **Save the connection string** for later use

#### Step 2: Create Redis Cache

1. Go to Render Dashboard → "New +" → "Redis"
2. Configure:
   - **Name**: `dineprox-redis`
   - **Plan**: Choose based on your needs (Free tier available)
3. Click "Create Redis"
4. **Save the connection string** for later use

#### Step 3: Deploy Web Service

1. Go to Render Dashboard → "New +" → "Web Service"
2. **Connect your repository**
3. Configure the service:
   - **Name**: `dineprox-api`
   - **Environment**: `Docker`
   - **Dockerfile Path**: `./Dockerfile.prod`
   - **Docker Context**: `.`
   - **Health Check Path**: `/health`

4. **Add Environment Variables**:
   ```
   ASPNETCORE_ENVIRONMENT=Production
   ASPNETCORE_URLS=http://+:80
   ConnectionStrings__Default=[Your PostgreSQL connection string]
   Redis__Configuration=[Your Redis connection string]
   ```

5. Click "Create Web Service"

## Environment Variables

### Required Variables

| Variable | Description | Example |
|----------|-------------|---------|
| `ASPNETCORE_ENVIRONMENT` | .NET environment | `Production` |
| `ASPNETCORE_URLS` | Application URLs | `http://+:80` |
| `ConnectionStrings__Default` | PostgreSQL connection | `Host=...;Database=...;Username=...;Password=...` |
| `Redis__Configuration` | Redis connection | `redis://...` |

### PostgreSQL Connection String Format

```
Host=your-postgres-host.render.com;Database=dineprox;Username=dineprox_user;Password=your-password;Port=5432;SSL Mode=Require;
```

### Redis Connection String Format

```
redis://username:password@your-redis-host.render.com:6379
```

## Database Migration

The application will automatically run database migrations on startup. If you need to run migrations manually:

1. **Access your web service logs** in Render Dashboard
2. **Check for migration output** during startup
3. **If migrations fail**, you can run them manually using the DbMigrator

## Monitoring and Logs

### View Logs
- Go to your web service in Render Dashboard
- Click on "Logs" tab
- Monitor for any startup issues or errors

### Health Checks
- Render automatically checks `/health` endpoint
- Ensure your application responds to health checks
- Monitor service status in the dashboard

## Scaling

### Auto-scaling
- Render provides automatic scaling based on traffic
- Configure scaling rules in your service settings
- Monitor resource usage in the dashboard

### Manual Scaling
- Upgrade your service plan for more resources
- Adjust memory and CPU allocations
- Consider upgrading database and Redis plans

## Security

### Environment Variables
- Never commit sensitive data to your repository
- Use Render's environment variable management
- Rotate database passwords regularly

### SSL/TLS
- Render provides automatic SSL certificates
- Your application will be accessible via HTTPS
- Configure your application to trust Render's certificates

## Troubleshooting

### Common Issues

1. **Database Connection Failed**
   - Check PostgreSQL connection string
   - Verify database is running
   - Check firewall settings

2. **Application Won't Start**
   - Check Docker build logs
   - Verify environment variables
   - Check application logs

3. **Migrations Failed**
   - Check database permissions
   - Verify connection string format
   - Check migration logs

### Debug Commands

```bash
# Check application logs
curl -f https://your-app.onrender.com/health

# Test database connection
# Use Render's database console or connection tools
```

## Performance Optimization

### Database Optimization
- Use connection pooling
- Optimize queries
- Consider read replicas for heavy read workloads

### Application Optimization
- Enable caching with Redis
- Optimize Docker image size
- Use CDN for static assets

### Monitoring
- Set up alerts for service health
- Monitor response times
- Track error rates

## Cost Optimization

### Free Tier
- PostgreSQL: 1GB storage, 90 days
- Redis: 25MB storage
- Web Service: 750 hours/month

### Paid Plans
- Scale based on your needs
- Monitor usage to optimize costs
- Consider reserved instances for predictable workloads

## Backup and Recovery

### Database Backups
- Render provides automatic PostgreSQL backups
- Configure backup retention policies
- Test restore procedures regularly

### Application Backups
- Your code is in version control
- Environment variables are managed by Render
- Consider backing up configuration files

## Support

- **Render Documentation**: [docs.render.com](https://docs.render.com/)
- **Community Support**: Render Discord/Slack
- **Technical Support**: Available on paid plans

## Next Steps

1. **Set up monitoring** and alerting
2. **Configure custom domain** if needed
3. **Set up CI/CD** for automatic deployments
4. **Implement logging** and error tracking
5. **Plan for scaling** as your application grows 