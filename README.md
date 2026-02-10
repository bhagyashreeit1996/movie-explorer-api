# 🎬 MovieExplorer API

MovieExplorer is a RESTful Web API built with **ASP.NET Core** and **Entity Framework Core** that allows users to explore movies, search by title, and manage movie likes.

---

---

## 📂 Project Structure (Enterprise Ready)

The project follows a tiered monolithic structure to separate concerns and ensure high maintainability:

- **`MovieExplorer.API/Core/`**: The heart of the app (Models, Interfaces).
- **`MovieExplorer.API/Application/`**: The business logic (Services, DTOs).
- **`MovieExplorer.API/Infrastructure/`**: The technical data layer (DbContext, Repos, Migrations).

---

## 📖 Project Guides

- **[🍎 macOS Setup Guide](./mac/README.md)**: How to run the project locally with Docker.
- **[🚀 Features & Roadmap](./features/README.md)**: What this API currently does and what's next.

---

## 🛠️ Adding a New Feature (Onboarding)

When adding a new feature (e.g., "Users" or "Reviews"), follow these steps to maintain our enterprise structure:

1.  **Define the Model**: Create a new class in `MovieExplorer.API/Core/Models`.
2.  **Contract (Interface)**: Define your repository/service needs in `MovieExplorer.API/Core/Interfaces`.
3.  **Update Database**: Add a `DbSet` to `MovieExplorer.API/Infrastructure/Data/ApplicationDbContext.cs`.
4.  **Implement Data Access**: Write the database logic in `MovieExplorer.API/Infrastructure/Repositories`.
5.  **Wire Up**: Register your new logic in `MovieExplorer.API/Extensions/ServiceCollectionExtensions.cs`.
6.  **Create Endpoint**: Add a new Controller in `Controllers/` that uses your interface.

---

## 🔧 Troubleshooting

If you change a model or pull new changes:
1. **Create a migration**: `dotnet ef migrations add <Name>`
2. **Apply to DB**: `dotnet ef database update`
*(Note: Always run EF commands from the `MovieExplorer.API` directory.)*
