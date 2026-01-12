# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Install Node.js and npm for Tailwind CSS build
RUN apt-get update && \
    apt-get install -y curl && \
    curl -fsSL https://deb.nodesource.com/setup_20.x | bash - && \
    apt-get install -y nodejs && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# Copy csproj and restore dependencies
COPY AgroProcessing.csproj ./
RUN dotnet restore

# Copy package.json and install npm dependencies
COPY package.json ./
RUN npm install

# Copy all project files
COPY . .

# Build Tailwind CSS
RUN npm run build:css

# Build .NET application
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false


# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AgroProcessing.dll"]
