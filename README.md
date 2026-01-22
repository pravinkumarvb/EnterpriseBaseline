# Enterprise Baseline – ASP.NET Core Backend

> **An enterprise-ready backend foundation focused on correctness, security, and long-term maintainability.**  
> This repository is not a demo and not a tutorial — it is a **baseline** designed to be extended safely by teams, enterprises, and AI-assisted development.

---

## 📌 What This Project Is

This project provides a **clean, opinionated, enterprise-grade backend baseline** built with:

- ASP.NET Core
- Entity Framework Core
- SQL Server
- JWT Authentication
- Permission-based Authorization
- Clean Architecture

It intentionally focuses on **architecture, security, and patterns**, not feature bloat.

---

## ❌ What This Project Is NOT

- ❌ Not a SaaS product
- ❌ Not a UI-heavy demo
- ❌ Not ASP.NET Identity–based
- ❌ Not opinionated about email, OTP, or UX flows
- ❌ Not a complete user lifecycle system

This is a **foundation**, not a finished product.

---

## 🧠 Core Design Principles

### 1. Clean Architecture

Dependencies flow **inward only**:

API → Application → Domain → Infrastructure


- Domain has no dependencies
- Application depends only on abstractions
- Infrastructure implements interfaces
- API wires everything together

---

### 2. Explicit Over Magic

- No framework-driven identity tables
- No hidden behaviors
- No scaffolding-generated logic

Every important decision is:
- Explicit
- Reviewable
- Replaceable

---

### 3. API-First Security

- JWT-based authentication
- Permission-based authorization
- Stateless design
- Ready for enterprise SSO / external IdPs

---

### 4. Enterprise-Safe Soft Delete

- All entities inherit `BaseEntity`
- Soft delete enforced via **global query filters**
- No scattered `IsDeleted` checks
- Deleted data remains auditable

---

## 🔐 Authentication & Authorization

### Authentication

- Username or Email + Password
- Secure password hashing
- JWT access tokens
- Stateless API authentication

### Authorization

- Permission-based (not role-only)
- Policies enforced via `[Authorize(Policy = "...")]`
- Permissions embedded in JWT claims
- Authorization occurs **before controller execution**

### Why NOT ASP.NET Identity?

This baseline intentionally avoids ASP.NET Identity because:

- Identity is cookie-first, not API-first
- Heavy schema and framework lock-in
- Enterprises often use external identity systems
- Explicit security is easier to audit and customize

---

## 🗂 Project Structure
```
EnterpriseBaseline
│
├── Api
│ ├── Controllers
│ ├── Authorization
│ ├── Middleware
│
├── Application
│ ├── DTOs
│ ├── Interfaces
│ ├── Services
│ ├── Exceptions
│ ├── Common
│
├── Domain
│ ├── Entities
│ └── Base
│
├── Infrastructure
│ ├── Persistence
│ ├── Repositories
│ └── Identity

```
---

## 📦 Included Features (v1)

### Backend Infrastructure

- Clean Architecture
- EF Core + SQL Server
- Database migrations
- Global exception handling
- Standard API response structure

### Security

- JWT authentication
- Permission-based authorization
- Policy-based access control
- Secure password hashing

### Reference Module

**Department Master**
- CRUD operations
- Permission-protected endpoints
- Soft delete
- Validation and uniqueness enforcement

This module serves as the **reference blueprint** for building additional features.

---

## 🚫 Intentionally Excluded (v1)

The following features are **intentionally not included**:

- User self-registration
- Change password
- Reset password / email flows
- OTP or email infrastructure
- OAuth / social login
- Frontend UI

These are **product-specific concerns**, not baseline responsibilities.

---

## 🧭 Roadmap

Planned future extensions:

- Admin-driven user management
- Change password & reset password APIs
- External IdP integration (Azure AD / Okta)
- Multi-tenancy support
- Angular frontend starter

---

## ▶️ Running the Project Locally

### Prerequisites

- .NET SDK
- SQL Server
- Visual Studio 2022 Community

### Steps

1. Update the connection string in `appsettings.json`
2. Run EF Core migrations
3. Start the API
4. Login using the seeded admin user

```json
{
  "userNameOrEmail": "admin",
  "password": "Admin@123"
}
```

5. Use the JWT token in Swagger Authorization

---

## 🧪 Authorization Behavior
- Missing JWT → 401 Unauthorized
- Valid JWT, missing permission → 403 Forbidden
- Valid permission → 200 OK<br/>
This behavior is intentional and enterprise-correct.

---

## 🤖 AI Usage Guidance
When using AI tools (e.g., ChatGPT):
- Follow existing patterns
- Do not access DbContext from Application layer
- Always add permissions and policies for new endpoints
- Keep controllers thin
- Preserve clean architecture boundaries<br/>
This structure is designed to remain safe even with AI-assisted development.

---

## 🧩 Adding a New Module
1. Create Domain entity
2. Add repository interface (Application)
3. Implement repository (Infrastructure)
4. Create service (Application)
5. Add controller (API)
6. Define permissions
7. Register authorization policies
8. Secure endpoints<br/>
Use the Department module as a reference.

---

## 🎯 Target Audience
- Enterprise developers
- Backend architects
- Internal enterprise systems
- Scalable SaaS foundations
- Long-term maintainable projects

---

## 🧠 Final Note
> This repository prioritizes clarity, control, and correctness over convenience.


### If you are looking for:
- A quick demo → this is not it
- A scalable, enterprise-safe backend foundation → this is exactly it

