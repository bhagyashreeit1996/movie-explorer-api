FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copy only project file first
COPY MovieExplorer.API.csproj ./

# Restore NuGet packages
RUN dotnet restore MovieExplorer.API.csproj

# Copy remaining source
COPY . .

# Publish application
RUN dotnet publish MovieExplorer.API.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "MovieExplorer.API.dll"]