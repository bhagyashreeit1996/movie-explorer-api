# 🍎 macOS Setup Guide

This folder contains the tools needed to run the MovieExplorer API on macOS with a local SQL Server instance.

## Prerequisites
- **Docker Desktop**: [Download and Install](https://www.docker.com/products/docker-desktop/)
- **.NET 8.0 SDK**: [Download and Install](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

## Quick Start
1.  **Open Docker Desktop** and ensure the engine is running.
2.  **Run the automated setup script**:
    ```bash
    chmod +x setup-mac.sh
    ./setup-mac.sh
    ```

## Files in this Folder
- `setup-mac.sh`: One-click script to start the DB, install EF tools, and apply migrations.
- `docker-compose.yml`: Configures the SQL Server 2022 container.

---
[⬅️ Back to main README](../README.md)
