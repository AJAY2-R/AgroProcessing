# ?? Docker Deployment Guide - AgroProcessing with SQLite

## ?? Overview

This guide explains how to deploy AgroProcessing with SQLite database in Docker, ensuring data persistence.

---

## ??? Database Configuration

### Development (Local)
- **Location**: `./agroprocessing.db` (project root)
- **Connection**: `Data Source=agroprocessing.db`

### Production (Docker)
- **Location**: `/app/data/agroprocessing.db` (inside container)
- **Volume Mount**: `./data` (on host) ? `/app/data` (in container)
- **Connection**: `Data Source=/app/data/agroprocessing.db`

---

## ?? Quick Start

### Option 1: Using Docker Compose (Recommended)

```bash
# 1. Build and start the container
docker-compose up -d

# 2. View logs
docker-compose logs -f

# 3. Access the application
# Open browser: http://localhost:8080
# Swagger UI: http://localhost:8080/swagger
```

### Option 2: Using Docker Commands

```bash
# 1. Build the image
docker build -t agroprocessing:latest ./AgroProcessing

# 2. Create a volume for database persistence
docker volume create agroprocessing-data

# 3. Run the container
docker run -d \
  --name agroprocessing-app \
  -p 8080:8080 \
  -v agroprocessing-data:/app/data \
  -e ASPNETCORE_ENVIRONMENT=Production \
  agroprocessing:latest

# 4. View logs
docker logs -f agroprocessing-app
```

---

## ?? File Structure

```
AgroProcessing/
??? AgroProcessing/
?   ??? Dockerfile                          # Docker configuration
?   ??? appsettings.json                    # Default settings
?   ??? appsettings.Development.json        # Development settings
?   ??? appsettings.Production.json         # Production settings (Docker)
?   ??? ...
??? docker-compose.yml                      # Docker Compose configuration
??? data/                                   # Database volume (created automatically)
?   ??? agroprocessing.db                   # SQLite database file
??? DOCKER_DEPLOYMENT.md                    # This file
```

---

## ?? Database Migration Process

### Automatic Migration (Recommended)

The application **automatically applies migrations** on startup:

1. ? Checks if `/app/data` directory exists (creates if missing)
2. ? Applies all pending EF Core migrations
3. ? Creates database if it doesn't exist
4. ? Logs migration status

**No manual intervention required!**

### Manual Migration (If Needed)

If you need to run migrations manually:

```bash
# 1. Access the container
docker exec -it agroprocessing-app bash

# 2. Navigate to app directory
cd /app

# 3. Check database status
ls -la /app/data/

# 4. Exit container
exit
```

---

## ?? Data Persistence

### How Data is Persisted

#### Using Docker Compose:
```yaml
volumes:
  - ./data:/app/data  # Host directory mapped to container
```

- Database stored in `./data/agroprocessing.db` on your host machine
- Survives container restarts and removals
- Can be backed up easily

#### Using Docker Volume:
```bash
docker volume create agroprocessing-data
docker run -v agroprocessing-data:/app/data ...
```

- Database stored in Docker-managed volume
- Survives container restarts and removals
- Use `docker volume inspect` to find location

---

## ?? Common Operations

### View Application Logs
```bash
# Using docker-compose
docker-compose logs -f

# Using docker
docker logs -f agroprocessing-app
```

### Restart Application
```bash
# Using docker-compose
docker-compose restart

# Using docker
docker restart agroprocessing-app
```

### Stop Application
```bash
# Using docker-compose
docker-compose down

# Using docker (keeps data)
docker stop agroprocessing-app

# Using docker (removes container, keeps data)
docker rm agroprocessing-app
```

### Rebuild After Code Changes
```bash
# Using docker-compose
docker-compose down
docker-compose build --no-cache
docker-compose up -d

# Using docker
docker stop agroprocessing-app
docker rm agroprocessing-app
docker build -t agroprocessing:latest ./AgroProcessing
docker run -d --name agroprocessing-app -p 8080:8080 -v ./data:/app/data agroprocessing:latest
```

---

## ?? Backup and Restore

### Backup Database

```bash
# Method 1: Copy from host (if using bind mount)
cp ./data/agroprocessing.db ./backups/agroprocessing-backup-$(date +%Y%m%d).db

# Method 2: Copy from container
docker cp agroprocessing-app:/app/data/agroprocessing.db ./backups/agroprocessing-backup-$(date +%Y%m%d).db

# Method 3: Using docker volume backup
docker run --rm -v agroprocessing-data:/data -v $(pwd)/backups:/backup \
  ubuntu tar czf /backup/agroprocessing-backup-$(date +%Y%m%d).tar.gz -C /data .
```

### Restore Database

```bash
# Method 1: Copy to host directory (if using bind mount)
cp ./backups/agroprocessing-backup-20260111.db ./data/agroprocessing.db
docker-compose restart

# Method 2: Copy to running container
docker cp ./backups/agroprocessing-backup-20260111.db agroprocessing-app:/app/data/agroprocessing.db
docker restart agroprocessing-app

# Method 3: Using docker volume restore
docker run --rm -v agroprocessing-data:/data -v $(pwd)/backups:/backup \
  ubuntu tar xzf /backup/agroprocessing-backup-20260111.tar.gz -C /data
```

---

## ?? Troubleshooting

### Database File Not Found

**Symptoms**: Application starts but can't access database

**Solution**:
```bash
# Check if data directory exists
docker exec agroprocessing-app ls -la /app/data/

# Check permissions
docker exec agroprocessing-app chmod 777 /app/data/

# Restart container
docker restart agroprocessing-app
```

### Migration Errors

**Symptoms**: Migration fails on startup

**Solution**:
```bash
# View detailed logs
docker logs agroprocessing-app

# Access container and check
docker exec -it agroprocessing-app bash
ls -la /app/data/
cat /app/appsettings.Production.json

# If needed, remove corrupted database and restart
docker exec agroprocessing-app rm /app/data/agroprocessing.db
docker restart agroprocessing-app
```

### Tailwind CSS Not Loading

**Symptoms**: Styles missing in browser

**Solution**:
```bash
# Rebuild with no cache
docker-compose build --no-cache
docker-compose up -d

# Verify output.css exists
docker exec agroprocessing-app ls -la /app/wwwroot/css/output.css
```

### Port Already in Use

**Symptoms**: Can't bind to port 8080

**Solution**:
```bash
# Check what's using port 8080
netstat -ano | findstr :8080  # Windows
lsof -i :8080                  # Linux/Mac

# Use different port
docker run -p 8081:8080 ...
# or in docker-compose.yml: "8081:8080"
```

### Container Keeps Restarting

**Symptoms**: Container restarts continuously

**Solution**:
```bash
# Check logs for errors
docker logs agroprocessing-app

# Run container without restart policy to see errors
docker run --rm -it -p 8080:8080 -v ./data:/app/data agroprocessing:latest

# Common issues:
# 1. Migration error - Check database permissions
# 2. Missing configuration - Check appsettings.Production.json
# 3. Port conflict - Use different port
```

---

## ?? Security Considerations

### Production Deployment

1. **Database Permissions**
   - Ensure `/app/data` has appropriate permissions (777 for development, more restrictive for production)
   - Set up proper file ownership

2. **Environment Variables**
   - Don't commit sensitive data to git
   - Use Docker secrets or environment files

3. **HTTPS**
   - Configure reverse proxy (nginx/traefik) for HTTPS
   - Update ASPNETCORE_URLS if needed

4. **Backups**
   - Set up automated backup schedule
   - Store backups securely off-server

---

## ?? Health Checks

The application includes a health check endpoint:

```yaml
healthcheck:
  test: ["CMD", "curl", "-f", "http://localhost:8080/swagger"]
  interval: 30s
  timeout: 10s
  retries: 3
```

Check health status:
```bash
docker ps  # Look at STATUS column
docker inspect agroprocessing-app | grep -A 10 Health
```

---

## ?? Accessing the Application

After successful deployment:

- **Web Interface**: http://localhost:8080
- **Swagger API**: http://localhost:8080/swagger
- **API Endpoints**: http://localhost:8080/api/*

---

## ?? Monitoring

### View Container Stats
```bash
docker stats agroprocessing-app
```

### Check Disk Usage
```bash
# Check volume size
docker system df -v

# Check database size
du -sh ./data/agroprocessing.db
```

---

## ?? Production Deployment Checklist

- [ ] Update `appsettings.Production.json` with production settings
- [ ] Configure proper database backup strategy
- [ ] Set up reverse proxy with HTTPS
- [ ] Configure proper logging and monitoring
- [ ] Test database migrations
- [ ] Test volume persistence
- [ ] Set up automated backups
- [ ] Configure restart policies
- [ ] Document connection strings
- [ ] Set up health monitoring
- [ ] Configure firewall rules
- [ ] Test disaster recovery

---

## ?? Support

If you encounter issues:

1. Check the logs: `docker logs agroprocessing-app`
2. Verify database exists: `docker exec agroprocessing-app ls -la /app/data/`
3. Check configuration: `docker exec agroprocessing-app cat /app/appsettings.Production.json`
4. Review this guide for common solutions

---

## ?? Summary

? **Database Location**: `/app/data/agroprocessing.db` (in container)  
? **Data Persistence**: Docker volume at `./data` (on host)  
? **Auto Migration**: Applied on every startup  
? **Tailwind CSS**: Built during Docker build  
? **Swagger**: Available at `/swagger`  
? **Port**: 8080 (configurable)  

**Your AgroProcessing application is now fully containerized with persistent SQLite database!** ??
