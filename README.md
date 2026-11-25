# Minecraft Content Hub

**Minecraft Content Hub** is a full-featured ASP.NET Core MVC web application designed as a dark-themed Content Management System (CMS) and catalog portal for Minecraft community content. Developed and expanded as the final individual project for the **SoftUni ASP.NET Advanced** course, it builds upon the original Fundamentals foundation with a layered service architecture, role-based administration area, pagination, automated database seeding, security hardening, and comprehensive NUnit test coverage.

---

## Project Concept & Core Features

The platform enables players to discover, search, and browse Minecraft additions across three dedicated catalogs, while administrators manage content and user permissions through a protected MVC Area.

### Public & User Features
* **Three Content Catalogs:**
  * **Mods (`ModProjects`):** Track title, description, author, download count, category, game version, mod loader (Fabric, Forge, NeoForge), and environment (Client / Server / Both).
  * **Resource Packs (`ResourcePacks`):** Organize texture packs by category, feature set, resolution (from `8x` up to `512x+`), and compatible game version.
  * **Data Packs (`DataPacks`):** Browse custom gameplay and world-generation data packs with version and category metadata.
* **Home Showcase:** Displays the latest additions across all three content types.
* **Search & Filtering:** Real-time filtering by title or author across all catalogs while preserving filter state during navigation.
* **Pagination:** Clean, bounds-safe pagination (`PaginatedList<T>`) across all catalogs with query persistence.
* **Custom Dark UI:** Responsive Bootstrap-based dark theme with reusable Razor Partial Views, layout sections, and custom-styled Identity pages.
* **Custom Error Handling:** Dedicated dark-themed **404 Not Found** and **500 Internal Server Error** pages configured via status code re-execution and global exception handling.

### Administrative Features (`Admin` MVC Area)
* **Admin Dashboard (`/Admin/Dashboard`):** Displays live entity counts (Mods, Resource Packs, Data Packs, Registered Users) and quick-action links for content creation.
* **User & Role Management (`/Admin/Users`):** Lists all registered users and their assigned roles, allowing administrators to grant or revoke the `Administrator` role (with built-in protection against removing one's own admin rights).
* **Full CRUD Operations:** Administrators can create, edit, and delete Mods, Resource Packs, and Data Packs.

---

## Tech Stack

* **Framework:** ASP.NET Core MVC (.NET 10.0)
* **Language:** C#
* **ORM & Database:** Entity Framework Core & Microsoft SQL Server (LocalDB)
* **Authentication & Authorization:** ASP.NET Core Identity (Roles: `User`, `Administrator`)
* **Front-End:** Razor Views, Partial Views, Sections, HTML5, CSS3, Bootstrap 5, jQuery Validation
* **Testing:** NUnit & `Microsoft.EntityFrameworkCore.InMemory`

---

## Architecture & Design Decisions

The solution is structured into two projects (`ASP.NET-FinalProject` and `ASP.NET_FinalProject.Tests`) following SOLID principles, loose coupling, strong cohesion, and clear separation of concerns.

```text
ASP.NET-FinalProject
├── Areas
│   ├── Admin                    # Dedicated MVC Area for Administrators
│   │   ├── Controllers          # AdminBaseController, DashboardController, UsersController
│   │   ├── Models               # AdminDashboardViewModel, UserViewModel
│   │   └── Views                # Dashboard & User Management views
│   └── Identity                 # Customized dark-themed Login & Register pages
├── Controllers                  # Thin MVC controllers delegating logic to Services
│   ├── HomeController.cs
│   ├── ModProjectsController.cs
│   ├── ResourcePacksController.cs
│   └── DataPacksController.cs
├── Data                         # ApplicationDbContext, Migrations, DbSeeder
├── Models                       # 10 Entity Models + PaginatedList<T>
├── Services                     # Business logic layer & Dependency Injection contracts
│   ├── Interfaces               # IModProjectService, IResourcePackService, IDataPackService
│   ├── ModProjectService.cs
│   ├── ResourcePackService.cs
│   └── DataPackService.cs
├── Views                        # Razor Views, Partial Views, Shared Error404/Error500
└── Program.cs                   # DI container, Middleware pipeline, Routing, Startup seeding

ASP.NET_FinalProject.Tests       # Dedicated NUnit test project
├── ModProjectServiceTests.cs
├── ResourcePackServiceTests.cs
├── DataPackServiceTests.cs
└── PaginatedListTests.cs
```

### Architectural Layers
1. **Presentation Layer (Controllers & Razor Views):** Controllers handle HTTP routing, model binding, authorization, and view selection. No controller interacts directly with `ApplicationDbContext`. Reusable card layouts and scripts are modularized using Razor Partial Views and `@section Scripts`.
2. **Service Layer (`Services/`):** Encapsulates all querying, filtering, pagination, eager loading (`.Include()`), lookup retrieval, and persistence logic behind interfaces (`IModProjectService`, `IResourcePackService`, `IDataPackService`) registered as scoped services in ASP.NET Core's built-in Dependency Injection container.
3. **Data Layer (`Data/` & `Models/`):** Consists of `ApplicationDbContext` (inheriting `IdentityDbContext`) and 10 relational entity models:
   * Primary entities: `ModProject`, `ResourcePack`, `DataPack`
   * Lookup entities: `Category`, `GameVersion`, `ModLoader`, `ResourceCategory`, `ResourceFeature`, `ResourceResolution`, `DataPackCategory`
4. **Admin Area (`Areas/Admin/`):** Uses an abstract `AdminBaseController` decorated with `[Area("Admin")]` and `[Authorize(Roles = "Administrator")]` to enforce DRY security rules across administrative endpoints.

---

## Validation & Security

* **Multi-Layer Data Validation:**
  * **Server-Side & Database Level:** Enforced via Data Annotations (`[Required]`, `[StringLength]` with minimum and maximum bounds, `[Url]`, `[Range(0, int.MaxValue)]`) and `ModelState.IsValid` checks in every POST action.
  * **Client-Side:** Implemented with jQuery Unobtrusive Validation via `_ValidationScriptsPartial` in all `Create` and `Edit` views for immediate user feedback.
* **CSRF Protection:** Enabled globally via `AutoValidateAntiforgeryTokenAttribute` in `Program.cs` alongside explicit `[ValidateAntiForgeryToken]` attributes and anti-forgery tokens in forms.
* **XSS Prevention:** All dynamic content is rendered through Razor's default HTML encoding (no raw unescaped user input), neutralizing special HTML tags such as `<script>` and `<br />`.
* **SQL Injection Prevention:** All database operations execute through Entity Framework Core LINQ queries with parameterized SQL.
* **Role-Based Access Control:** Administrative CRUD endpoints and the entire `Admin` Area require the `Administrator` role; unauthorized users are redirected to Login or Access Denied.
* **Bounds-Safe Pagination:** `PaginatedList<T>` automatically clamps out-of-range page numbers (e.g., negative or excessively high values) to valid bounds to prevent application errors.

---

## Database Seeding

The project provides two complementary seeding mechanisms so the application runs immediately with rich, realistic data:

1. **Migration-Level Lookup Seeding (`ApplicationDbContext.OnModelCreating`):** Automatically populates all reference tables via `HasData`, including 3 Mod Loaders, 19 Mod Categories, 100+ Minecraft Game Versions, 10 Resource Categories, 11 Resource Features, 8 Resource Resolutions, and 19 Data Pack Categories.
2. **Idempotent Startup Seeding (`Data/DbSeeder.cs`):** Executed automatically on application startup to ensure:
   * Creation of the `Administrator` and `User` roles.
   * Creation of ready-to-use test accounts:
     * **Administrator:** `admin@mcportal.com` / `Admin123!`
     * **Standard User:** `user@mcportal.com` / `User123!`
   * Seeding of **21 sample items** (7 Mods, 7 Resource Packs, and 7 Data Packs) when tables are empty — ensuring every catalog immediately demonstrates search, filtering, and multi-page pagination (`6` items per page).

---

## Unit Testing & Test Coverage

Unit tests are located in the dedicated **`ASP.NET_FinalProject.Tests`** project using **NUnit** and **EF Core In-Memory Database** with isolated database instances per test (`Guid.NewGuid()`).

| Test Fixture | Tests | Covered Logic |
| --- | :---: | --- |
| `ModProjectServiceTests` | 14 | `GetAllAsync` (all, filter by title/author), `GetPaginatedAsync`, `GetByIdWithDetailsAsync`, `GetByIdAsync`, `CreateAsync`, `UpdateAsync`, `DeleteAsync` (existing & non-existing), `ExistsAsync`, lookup methods |
| `ResourcePackServiceTests` | 14 | Full CRUD, search/filtering, pagination, navigation property loading, existence checks, and lookup retrieval |
| `DataPackServiceTests` | 14 | Full CRUD, search/filtering, pagination, navigation property loading, existence checks, and lookup retrieval |
| `PaginatedListTests` | 6 | First/middle/last page slicing, negative/zero page clamping, out-of-bounds clamping, empty query handling |
| **Total** | **48** | **All 48 unit tests passing (100% of public service methods covered, exceeding the 65% requirement)** |

### Running the Tests
* **Visual Studio:** Open **Test -> Test Explorer** and click **Run All Tests**.
* **CLI:** Run the following command from the solution root:
  ```bash
  dotnet test
  ```

---

## How to Run Locally

No external API keys or third-party services are required.

1. **Clone the repository:**
   ```bash
   git clone https://github.com/StoryBuild/ASP.NET-FinalProject.git
   ```

2. **Open the solution:**
   Open `ASP.NET-FinalProject.slnx` in Visual Studio 2022 (or JetBrains Rider).

3. **Verify the connection string:**
   `appsettings.json` is pre-configured for Visual Studio's default SQL Server LocalDB:
   ```json
   "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=MC_ModsDb;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False"
   ```

4. **Apply Entity Framework Core Migrations:**
   Open the **Package Manager Console** in Visual Studio (with `ASP.NET-FinalProject` selected as the Default Project) and run:
   ```powershell
   Update-Database
   ```

5. **Run the application:**
   Press `F5` (or run `dotnet run` inside the web project folder). On first startup, `DbSeeder` will automatically create the roles, default user accounts, and sample Minecraft content.

6. **Explore with seeded accounts:**
   * Browse catalogs, search, and paginate as a guest or log in as `user@mcportal.com` (`User123!`).
   * Log in as `admin@mcportal.com` (`Admin123!`) to access the **Admin Panel** (`/Admin/Dashboard`), manage user roles, and perform CRUD operations.

---

**Author:** StoryBuild  
*Developed for educational purposes as part of the SoftUni ASP.NET Core Advanced curriculum.*