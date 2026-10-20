# Minecraft Content Hub


## About the Project

I built this web app as my final project for the ASP.NET Core Fundamentals course. Since I play a lot of Minecraft, I wanted to create a content catalog where users can easily browse and manage their favorite game additions. 

The application serves as a CMS (Content Management System) where you can manage three main types of content:
* **Mods:** Track title, author, game version, mod loader, and environment (Client/Server).
* **Resource Packs:** Organize texture packs by resolution and features.
* **Data Packs:** Manage custom data packs and their metadata.

My main goal was to put what we learned in the course into practice—building a solid MVC architecture, using Entity Framework Core for data persistence, handling CRUD operations, and creating a clean, responsive dark-themed UI with Bootstrap.

## Tech Stack

* ASP.NET Core MVC (.NET 10.0)
* C#
* Entity Framework Core
* SQL Server (LocalDB)
* HTML, CSS, Bootstrap & Razor Views

## How to Run It Locally

You don't need any complex setup, third-party authentication, or external API keys to test this out. 

1. **Clone the repo:**
   ```bash
   git clone https://github.com/StoryBuild/ASP.NET-FinalProject.git
   ```

2. **Open the project:**
   Open the `ASP.NET-FinalProject.slnx` file in Visual Studio 2022.

3. **Database Setup:**
   The connection string in `appsettings.json` is already configured to use Visual Studio's default LocalDB. You don't need to change anything if you are grading or testing the app.
   ```json
   "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=MC_ModsDb;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False"
   ```

4. **Apply Migrations (Important):**
   Open the **Package Manager Console** in Visual Studio and run:
   ```powershell
   Update-Database
   ```
   *This will automatically create the database and seed it with all the default categories, game versions, and mod loaders so the app is ready to use immediately.*

5. **Run the app:**
   Press `F5` or click Start in Visual Studio.

## Architecture & Code Quality

The project strictly follows the ASP.NET MVC pattern, keeping Models, Views, and Controllers cleanly separated. To meet best practices and project requirements, the following concepts are implemented:

- **Server-side validation:** Enforced using data annotations (like `[Required]`, `[MaxLength]`, and `[Url]`) on the models to ensure data integrity.
- **Client-side validation:** Implemented using jQuery validation scripts via Razor `@section Scripts` to provide instant feedback on forms.
- **Reusable UI Components:** Applied **Razor Partial Views** to extract and reuse the card designs for Mods, Resource Packs, and Data Packs across the application.
---
**Author:** StoryBuild  
*Developed for educational purposes as part of the SoftUni ASP.NET Core curriculum.*