# ?? Quick Start - Docker with SQLite

## ? Fastest Way to Deploy

### Windows:
```cmd
deploy.bat
# Select option 1
```

### Linux/Mac:
```bash
chmod +x deploy.sh
./deploy.sh
# Select option 1
```

---

## ?? Essential Commands

### Deploy
```bash
# Using docker-compose (recommended)
docker-compose up -d

# Using docker
docker run -d --name agroprocessing-app -p 8080:8080 -v ./data:/app/data agroprocessing:latest
```

### Access
- **Web App**: http://localhost:8080
- **API Docs**: http://localhost:8080/swagger

### Logs
```bash
docker-compose logs -f
# or
docker logs -f agroprocessing-app
```

### Stop
```bash
docker-compose down
# or
docker stop agroprocessing-app
```

### Backup
```bash
cp ./data/agroprocessing.db ./backups/backup-$(date +%Y%m%d).db
```

---

## ?? Where is My Database?

- **Host**: `./data/agroprocessing.db`
- **Container**: `/app/data/agroprocessing.db`

Database persists even when container is removed!

---

## ? What's Automatic?

? Database migrations (applied on startup)  
? Directory creation  
? Tailwind CSS compilation  
? Health checks  

---

## ?? Troubleshooting

### Can't access app?
```bash
docker logs agroprocessing-app
```

### Database issues?
```bash
docker exec agroprocessing-app ls -la /app/data/
```

### Need to rebuild?
```bash
docker-compose build --no-cache
docker-compose up -d
```

---

## ?? Full Documentation

See `DOCKER_DEPLOYMENT.md` for complete guide.
