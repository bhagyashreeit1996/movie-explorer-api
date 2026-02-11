#!/bin/bash

# MovieExplorer API - Quick Setup Script for Mac
# This script automates the setup process focusing on SQL Server via Docker

set -e  # Exit on error

echo "🎬 MovieExplorer API - Mac Setup Script"
echo "========================================"
echo ""

# Check if .NET is installed
echo "📦 Checking for .NET SDK..."
if ! command -v dotnet &> /dev/null; then
    echo "❌ .NET SDK not found!"
    echo "Please install .NET 8 SDK:"
    echo "  brew install --cask dotnet-sdk"
    echo "  OR download from: https://dotnet.microsoft.com/download/dotnet/8.0"
    exit 1
fi

DOTNET_VERSION=$(dotnet --version)
echo "✅ .NET SDK found: $DOTNET_VERSION"
echo ""

echo "🗄️  Database Setup"
echo "🚀 Starting SQL Server with Docker..."
if ! command -v docker &> /dev/null; then
    echo "❌ Docker not found! Please install Docker Desktop first."
    exit 1
fi
docker-compose up -d
echo "✅ SQL Server is starting..."
echo ""

# Restore packages
echo "📦 Restoring NuGet packages..."
cd MovieExplorer.API
dotnet restore
echo "✅ Packages restored"
echo ""

# Check if EF tools are installed
echo "🔧 Checking Entity Framework tools..."
if ! dotnet ef &> /dev/null; then
    echo "Installing dotnet-ef..."
    dotnet tool install --global dotnet-ef
    echo "✅ Entity Framework tools installed"
else
    echo "✅ Entity Framework tools already installed"
fi
echo ""

# Run migrations
echo "🔄 Applying database migrations..."
echo "Waiting for SQL Server to be ready..."
sleep 15 # Give Docker some time to boot

if dotnet ef database update; then
    echo "✅ Database migrations applied"
else
    echo "⚠️  Migration failed. You may need to run 'dotnet ef database update' manually once Docker is fully ready."
fi
echo ""

cd ..

echo "✅ Setup Complete!"
echo ""
echo "🚀 To run the application:"
echo "   cd MovieExplorer.API"
echo "   dotnet run"
echo ""
echo "   Or with hot reload:"
echo "   dotnet watch run"
echo ""
echo "📖 Access Swagger UI at: http://localhost:5123/swagger"
echo ""
echo "📚 For more details, see README.md"
