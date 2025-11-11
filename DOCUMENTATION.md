# KoleosLite (KoleosDemo) - Project Documentation

Last updated:2025-11-11

## Overview
KoleosLite is an ASP.NET Core8 Blazor Server application for managing an automotive repair shop. It provides a REST API (EF Core + SQL Server) and a Blazor Server dashboard.

Features implemented:
- Entities: Cliente, Vehiculo, Empleado, Servicio, Repuesto, OrdenServicio, DetalleOrden, Factura, Usuario
- EF Core `ApplicationDbContext` with fluent model configuration and relationships
- REST API controllers for core entities and `UsuariosController` with secure password hashing (PBKDF2)
- Blazor Server UI with pages for Dashboard, Clientes, Vehiculos, Ordenes, Login, Register, Logout
- Demo authentication using `SimpleAuthStateProvider` (replaceable with ASP.NET Identity)
- Dark/Light theme support persisted in `localStorage`
- URL normalization middleware + client-side script to collapse duplicate slashes
- Basic repository pattern via `IRepository<T>` and `GenericRepository<T>`
- Service registration centralised in `Infrastructure/ServiceExtensions.cs`

## Security
- Passwords are hashed using PBKDF2 (`Rfc2898DeriveBytes`) with100,000 iterations and32-byte output. Stored as `Base64(salt):Base64(hash)`.
- Demo `SimpleAuthStateProvider` is for local demo only. For production, use ASP.NET Core Identity or a proper token-based authentication (JWT with refresh tokens) and TLS.

## Modularity & Performance
- Service registration moved to `Infrastructure/ServiceExtensions`.
- Generic repository provided for data access and easier unit testing.
- Blazor components use `HttpClient` registered with `NavigationManager.BaseUri` for server calls.
- Middlewares: URL normalization early in pipeline.

## Files & Structure
- `Program.cs` - app startup (uses `ServiceExtensions`)
- `ApplicationDbContext.cs` - EF Core context and model config
- `Controllers/` - API controllers
- `Pages/` - Blazor pages and `Shared` layout
- `Services/SimpleAuthStateProvider.cs` - demo authentication provider
- `Repositories/GenericRepository.cs` - basic repository interface & impl
- `Infrastructure/ServiceExtensions.cs` - centralized registrations
- `wwwroot/css/site.css` - theming and dark-mode variables
- `wwwroot/js/theme.js` - localStorage theme helper

## How to run
1. Ensure connection string in `appsettings.json` points to a reachable SQL Server.
2. From project folder (where .csproj is):
 - `dotnet tool install --global dotnet-ef` (if needed)
 - `dotnet ef migrations add InitialCreate`
 - `dotnet ef database update`
 - `dotnet run`
3. Open `https://localhost:7176/` (or port in launchSettings). Register a user and log in.

## Next steps / Improvements
- Replace `SimpleAuthStateProvider` with ASP.NET Core Identity (EF stores, password policies, email confirmation).
- Protect API endpoints with authorization attributes and role checks.
- Add unit and integration tests.
- Improve UI with a component library (MudBlazor) and server-side pagination.
- Implement connection resiliency, logging, and structured telemetry (Application Insights).

---

For detailed per-file explanations see the `src/` tree comments and source code.