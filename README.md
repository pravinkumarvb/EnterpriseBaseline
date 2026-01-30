# EnterpriseBaseline

> A production-ready **Angular + ASP.NET Core enterprise starter kit** with authentication, authorization, clean architecture, and a real reference CRUD feature.
> This project removes repetitive setup work and provides a solid, extensible foundation for building internal tools, admin panels, and line-of-business applications.

---

## 🎯 Who This Is For

- Enterprise developers
- Freelancers building admin dashboards
- Teams starting greenfield Angular + .NET projects
- Developers who value clean architecture and long-term maintainability

---

## 🚀 What This Project Provides

### Backend (ASP.NET Core)
- Clean Architecture (Domain, Application, Infrastructure, API)
- JWT Authentication
- Permission-based Authorization (policy-based)
- Global exception handling
- Soft delete support
- SQL Server with Entity Framework Core
- Reference CRUD feature (Department)

### Frontend (Angular)
- Angular 17 standalone architecture
- JWT authentication flow
- AuthGuard and PermissionGuard
- Global HTTP interceptor
- Feature-based folder structure
- Layout shell with navbar and logout
- Reference CRUD UI (Department)

---

## 🧱 Backend Architecture (ASP.NET Core)

The backend follows **Clean Architecture** principles.

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

### Layer Responsibilities

- **Api**: HTTP endpoints, authentication, authorization, middleware
- **Application**: Business logic, DTOs, interfaces, use cases
- **Domain**: Core business entities and rules
- **Infrastructure**: Database access, repositories, security implementations

---

## 🧩 Frontend Architecture (Angular)

The frontend uses a **feature-based structure** with standalone components.
```
src/app
├── core
│ ├── guards
│ ├── interceptors
│ ├── layouts
│ └── services
│
├── features
│ ├── auth
│ └── departments
│
├── shared
│ └── components
│
└── app.routes.ts
```

### Key Principles

- Feature-based organization
- Core for cross-cutting concerns
- Shared for reusable UI
- Standalone components (no NgModules)

---

## 🔐 Authentication & Authorization Flow

- Users authenticate using JWT
- Token stored securely in local storage
- HTTP interceptor attaches JWT to API requests
- AuthGuard protects authenticated routes
- PermissionGuard enforces fine-grained permissions
- Backend remains the final authority

---

## ▶️ Running the Project

### Backend
1. Open solution in Visual Studio 2022
2. Update `appsettings.json` connection string
3. Run database migrations
4. Start the API project

### Frontend
```bash
npm install
ng serve

Frontend runs at: http://localhost:4200
Backend runs at: https://localhost:7172
```
___

## 🧪 Reference Feature: Department
The Department feature demonstrates:
- End-to-end CRUD
- DTO-based API contracts
- Validation and error handling
- Permission-based access control
- Reusable frontend patterns

Use this feature as a template to build additional modules.

___

## 🧠 Design Philosophy
This project prioritizes:
- Clarity over cleverness
- Explicitness over magic
- Long-term maintainability over shortcuts

___

## 📌 License
MIT

___

## 📬 Feedback & Contributions
This is a living baseline.<br/>
Feedback, improvements, and discussions are welcome.

---




