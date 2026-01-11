#!/bin/bash

# AgroProcessing Deployment Script
# This script builds and deploys the application with SQLite database

set -e  # Exit on error

echo "?? AgroProcessing Deployment Script"
echo "===================================="
echo ""

# Check if docker is installed
if ! command -v docker &> /dev/null; then
    echo "? Docker is not installed. Please install Docker first."
    exit 1
fi

# Check if docker-compose is installed
if ! command -v docker-compose &> /dev/null; then
    echo "??  docker-compose is not installed. Using docker commands instead."
    USE_COMPOSE=false
else
    USE_COMPOSE=true
fi

echo "?? Deployment Options:"
echo "1. Deploy with docker-compose (recommended)"
echo "2. Deploy with docker commands"
echo "3. Rebuild and deploy"
echo "4. Stop and remove containers"
echo "5. View logs"
echo "6. Backup database"
echo "7. Restore database"
echo ""

read -p "Select option (1-7): " option

case $option in
    1)
        echo "?? Deploying with docker-compose..."
        if [ "$USE_COMPOSE" = true ]; then
            docker-compose up -d
            echo "? Deployment complete!"
            echo "?? Application: http://localhost:8080"
            echo "?? Swagger: http://localhost:8080/swagger"
            echo ""
            echo "View logs with: docker-compose logs -f"
        else
            echo "? docker-compose not available. Please install it or use option 2."
        fi
        ;;
    
    2)
        echo "?? Deploying with docker commands..."
        
        # Build image
        echo "?? Building Docker image..."
        docker build -t agroprocessing:latest ./AgroProcessing
        
        # Stop and remove existing container
        echo "?? Stopping existing container (if any)..."
        docker stop agroprocessing-app 2>/dev/null || true
        docker rm agroprocessing-app 2>/dev/null || true
        
        # Create data directory
        echo "?? Creating data directory..."
        mkdir -p ./data
        
        # Run container
        echo "??  Starting container..."
        docker run -d \
          --name agroprocessing-app \
          -p 8080:8080 \
          -v "$(pwd)/data:/app/data" \
          -e ASPNETCORE_ENVIRONMENT=Production \
          --restart unless-stopped \
          agroprocessing:latest
        
        echo "? Deployment complete!"
        echo "?? Application: http://localhost:8080"
        echo "?? Swagger: http://localhost:8080/swagger"
        echo ""
        echo "View logs with: docker logs -f agroprocessing-app"
        ;;
    
    3)
        echo "?? Rebuilding and deploying..."
        if [ "$USE_COMPOSE" = true ]; then
            docker-compose down
            docker-compose build --no-cache
            docker-compose up -d
            echo "? Rebuild complete!"
        else
            docker stop agroprocessing-app 2>/dev/null || true
            docker rm agroprocessing-app 2>/dev/null || true
            docker rmi agroprocessing:latest 2>/dev/null || true
            docker build --no-cache -t agroprocessing:latest ./AgroProcessing
            docker run -d --name agroprocessing-app -p 8080:8080 \
              -v "$(pwd)/data:/app/data" \
              -e ASPNETCORE_ENVIRONMENT=Production \
              --restart unless-stopped \
              agroprocessing:latest
            echo "? Rebuild complete!"
        fi
        ;;
    
    4)
        echo "?? Stopping and removing containers..."
        if [ "$USE_COMPOSE" = true ]; then
            docker-compose down
        else
            docker stop agroprocessing-app 2>/dev/null || true
            docker rm agroprocessing-app 2>/dev/null || true
        fi
        echo "? Containers stopped and removed"
        echo "?? Database preserved in ./data directory"
        ;;
    
    5)
        echo "?? Viewing logs..."
        if [ "$USE_COMPOSE" = true ]; then
            docker-compose logs -f
        else
            docker logs -f agroprocessing-app
        fi
        ;;
    
    6)
        echo "?? Backing up database..."
        BACKUP_DIR="./backups"
        mkdir -p "$BACKUP_DIR"
        BACKUP_FILE="$BACKUP_DIR/agroprocessing-backup-$(date +%Y%m%d-%H%M%S).db"
        
        if [ -f "./data/agroprocessing.db" ]; then
            cp ./data/agroprocessing.db "$BACKUP_FILE"
            echo "? Database backed up to: $BACKUP_FILE"
        else
            echo "? Database file not found at ./data/agroprocessing.db"
        fi
        ;;
    
    7)
        echo "?? Restore database..."
        echo "Available backups:"
        ls -lh ./backups/*.db 2>/dev/null || echo "No backups found"
        echo ""
        read -p "Enter backup filename (e.g., agroprocessing-backup-20260111-120000.db): " backup_file
        
        if [ -f "./backups/$backup_file" ]; then
            echo "??  This will overwrite the current database!"
            read -p "Are you sure? (yes/no): " confirm
            if [ "$confirm" = "yes" ]; then
                cp "./backups/$backup_file" ./data/agroprocessing.db
                echo "? Database restored from: $backup_file"
                echo "?? Restarting application..."
                if [ "$USE_COMPOSE" = true ]; then
                    docker-compose restart
                else
                    docker restart agroprocessing-app
                fi
                echo "? Application restarted"
            else
                echo "? Restore cancelled"
            fi
        else
            echo "? Backup file not found: ./backups/$backup_file"
        fi
        ;;
    
    *)
        echo "? Invalid option"
        exit 1
        ;;
esac

echo ""
echo "? Done!"
