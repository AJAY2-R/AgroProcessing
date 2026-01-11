@echo off
REM AgroProcessing Deployment Script for Windows
REM This script builds and deploys the application with SQLite database

echo.
echo ?? AgroProcessing Deployment Script
echo ====================================
echo.

REM Check if docker is installed
docker --version >nul 2>&1
if errorlevel 1 (
    echo ? Docker is not installed. Please install Docker Desktop first.
    pause
    exit /b 1
)

REM Check if docker-compose is installed
docker-compose --version >nul 2>&1
if errorlevel 1 (
    set USE_COMPOSE=false
    echo ??  docker-compose is not available. Using docker commands.
) else (
    set USE_COMPOSE=true
)

echo.
echo ?? Deployment Options:
echo 1. Deploy with docker-compose (recommended)
echo 2. Deploy with docker commands
echo 3. Rebuild and deploy
echo 4. Stop and remove containers
echo 5. View logs
echo 6. Backup database
echo 7. Restore database
echo.

set /p option="Select option (1-7): "

if "%option%"=="1" goto deploy_compose
if "%option%"=="2" goto deploy_docker
if "%option%"=="3" goto rebuild
if "%option%"=="4" goto stop
if "%option%"=="5" goto logs
if "%option%"=="6" goto backup
if "%option%"=="7" goto restore
goto invalid

:deploy_compose
echo.
echo ?? Deploying with docker-compose...
if "%USE_COMPOSE%"=="true" (
    docker-compose up -d
    echo ? Deployment complete!
    echo ?? Application: http://localhost:8080
    echo ?? Swagger: http://localhost:8080/swagger
    echo.
    echo View logs with: docker-compose logs -f
) else (
    echo ? docker-compose not available. Please use option 2.
)
goto end

:deploy_docker
echo.
echo ?? Deploying with docker commands...

echo ?? Building Docker image...
docker build -t agroprocessing:latest .\AgroProcessing

echo ?? Stopping existing container (if any)...
docker stop agroprocessing-app 2>nul
docker rm agroprocessing-app 2>nul

echo ?? Creating data directory...
if not exist "data" mkdir data

echo ??  Starting container...
docker run -d --name agroprocessing-app -p 8080:8080 -v "%cd%\data:/app/data" -e ASPNETCORE_ENVIRONMENT=Production --restart unless-stopped agroprocessing:latest

echo ? Deployment complete!
echo ?? Application: http://localhost:8080
echo ?? Swagger: http://localhost:8080/swagger
echo.
echo View logs with: docker logs -f agroprocessing-app
goto end

:rebuild
echo.
echo ?? Rebuilding and deploying...
if "%USE_COMPOSE%"=="true" (
    docker-compose down
    docker-compose build --no-cache
    docker-compose up -d
    echo ? Rebuild complete!
) else (
    docker stop agroprocessing-app 2>nul
    docker rm agroprocessing-app 2>nul
    docker rmi agroprocessing:latest 2>nul
    docker build --no-cache -t agroprocessing:latest .\AgroProcessing
    docker run -d --name agroprocessing-app -p 8080:8080 -v "%cd%\data:/app/data" -e ASPNETCORE_ENVIRONMENT=Production --restart unless-stopped agroprocessing:latest
    echo ? Rebuild complete!
)
goto end

:stop
echo.
echo ?? Stopping and removing containers...
if "%USE_COMPOSE%"=="true" (
    docker-compose down
) else (
    docker stop agroprocessing-app 2>nul
    docker rm agroprocessing-app 2>nul
)
echo ? Containers stopped and removed
echo ?? Database preserved in .\data directory
goto end

:logs
echo.
echo ?? Viewing logs...
if "%USE_COMPOSE%"=="true" (
    docker-compose logs -f
) else (
    docker logs -f agroprocessing-app
)
goto end

:backup
echo.
echo ?? Backing up database...
if not exist "backups" mkdir backups
set BACKUP_FILE=backups\agroprocessing-backup-%date:~-4,4%%date:~-10,2%%date:~-7,2%-%time:~0,2%%time:~3,2%%time:~6,2%.db
set BACKUP_FILE=%BACKUP_FILE: =0%

if exist "data\agroprocessing.db" (
    copy "data\agroprocessing.db" "%BACKUP_FILE%"
    echo ? Database backed up to: %BACKUP_FILE%
) else (
    echo ? Database file not found at data\agroprocessing.db
)
goto end

:restore
echo.
echo ?? Restore database...
echo Available backups:
dir /b backups\*.db 2>nul
if errorlevel 1 echo No backups found
echo.
set /p backup_file="Enter backup filename (e.g., agroprocessing-backup-20260111-120000.db): "

if exist "backups\%backup_file%" (
    echo.
    echo ??  This will overwrite the current database!
    set /p confirm="Are you sure? (yes/no): "
    if /i "%confirm%"=="yes" (
        copy "backups\%backup_file%" "data\agroprocessing.db"
        echo ? Database restored from: %backup_file%
        echo ?? Restarting application...
        if "%USE_COMPOSE%"=="true" (
            docker-compose restart
        ) else (
            docker restart agroprocessing-app
        )
        echo ? Application restarted
    ) else (
        echo ? Restore cancelled
    )
) else (
    echo ? Backup file not found: backups\%backup_file%
)
goto end

:invalid
echo ? Invalid option
goto end

:end
echo.
echo ? Done!
pause
