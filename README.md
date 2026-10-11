# AuthService

A modular ASP.NET Core Web API demonstrating user registration, JWT authentication, refresh-token rotation, and role-protected endpoints. The solution separates API, application contracts, domain logic, and infrastructure.

## Features
- User registration and login with password hashing and verification.
- JWT bearer authentication and refresh-token revocation/rotation.
- Protected user and Admin-role example endpoints.
- SQL Server persistence through Entity Framework Core.
- Layered solution structure for API, application, contracts, domain, and infrastructure.

## Tech stack
C# · .NET 8 · ASP.NET Core Web API · Entity Framework Core 8 · SQL Server · JWT Bearer · FluentValidation · Serilog

## Solution structure
- `Auth.API` — startup and HTTP controllers.
- `Auth.Application` — application interfaces.
- `Auth.Contracts` — request/response contracts.
- `Auth.Domain` — domain entities and rules.
- `Auth.Infrastructure` — database access, repositories, password hashing, and token services.

## Getting started
Prerequisites: .NET 8 SDK and SQL Server.

Configure `ConnectionStrings:DefaultConnection` and the `Jwt:Issuer`, `Jwt:Audience`, and `Jwt:Key` settings in local configuration or user secrets. Never commit real credentials or signing keys.

From the repository root:

```bash
dotnet restore
dotnet build
dotnet run --project Auth.API/Auth.API.csproj
```

Create/configure the database and apply any project migrations before testing database-backed operations.

## API routes
| Method | Route | Purpose |
|---|---|---|
| POST | `/api/auth/register` | Register a user |
| POST | `/api/auth/login` | Authenticate and issue tokens |
| POST | `/api/auth/refresh` | Refresh tokens |
| GET | `/api/secure` | Example authenticated route |
| GET | `/api/secure/admin` | Example Admin-role route |

Use JSON bodies matching the request types in `Auth.Contracts`.

## Security
Keep secrets out of source control, use HTTPS in deployed environments, and treat access/refresh tokens as credentials. Review validation, role assignment, expiry, and revocation behaviour before production deployment.
