# 1. Build Stage
# Yahan 8.0 ki jagah 9.0 kar diya hai
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

# Copy files
COPY *.sln .
COPY *.csproj ./
RUN dotnet restore

# Build
COPY . .
RUN dotnet publish -c Release -o /app

# 2. Runtime Stage
# Yahan bhi 9.0 hona chahiye
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .

# Render port configuration
EXPOSE 8080

# Replace 'TMS.dll' with your actual DLL name if it's different
ENTRYPOINT ["dotnet", "TMS.dll"]
