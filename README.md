# 🎬 MovieExplorer API

MovieExplorer is a RESTful Web API built with **ASP.NET Core** and **Entity Framework Core** that allows users to explore movies, search by title, and manage movie likes.

---

## 🌍 Database Strategy

To ensure **perfect consistency** with your Windows-based team, we use **SQL Server for everything**.

- **Mac (You):** Run SQL Server in Docker.
- **Windows:** Run SQL Server natively.

This ensures your migration files and database schema stay identical across the whole team.

---

## 🚀 Getting Started (Mac)

1. **Install Docker Desktop**.
2. **Run the setup script**:
   ```bash
   ./setup-mac.sh
   ```
3. **Start the app**:
   ```bash
   cd MovieExplorer.API
   dotnet run
   ```

---

## 💻 Team Workflow

- **Shared code:** Both platforms share the **exact same** migration files in git.
- **Consistent Schema:** No `TEXT` vs `nvarchar` issues.

---

## 🛠️ Tech Stack

- **.NET 8.0**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **SQL Server** (Docker on Mac / Native on Windows)
- **Swagger UI** (Access at `http://localhost:5123/swagger`)

---

## 📂 Project Structure

- `MovieExplorer.API/` - Main Web API project
- `setup-mac.sh` - Automation script for Mac setup
- `docker-compose.yml` - SQL Server container for Mac
- `.vscode/` - Recommended VS Code configurations (Debug with F5!)

---

## 🔧 Troubleshooting

If you run into database issues after pulling new changes:
```bash
dotnet ef database update
```
*(Requires `dotnet-ef` tool. The setup script installs this for you!)*
