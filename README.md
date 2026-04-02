# 🚗 CarMarketApp - Backend Infrastructure

Welcome to the **CarMarketApp Backend**, a highly scalable, robust, and modern RESTful API built for a comprehensive car marketplace platform. 

This project is thoughtfully engineered using the principles of **Clean Architecture** and the **CQRS (Command Query Responsibility Segregation)** design pattern to ensure maximum maintainability, testability, and separation of concerns.

---

## 🎯 Key Design Choices & Architecture

This repository strictly adheres to **Clean Architecture**. Dependencies always point inwards towards the Domain layer, making the core business logic entirely independent of UI, databases, and frameworks.

The solution is divided into four main projects:

1. **`CarMarketApp.Domain` (Core)**
   - **Purpose**: The heart of the application containing the Enterprise Business Rules.
   - **Contents**: Domain Entities (`Advert`, `Brand`, `Model`, `RefreshToken`, etc.) and core logical abstractions.
   - **Dependencies**: None.

2. **`CarMarketApp.Application` (Use Cases)**
   - **Purpose**: Contains the application-specific business rules.
   - **Contents**: 
     - CQRS Implementation (Commands, Queries, and Handlers using MediatR).
     - Data Transfer Objects (DTOs) for encapsulating data.
     - Custom `Result` and `Result<T>` Wrapper Pattern for standardized API responses containing `Success` status and error messages.
     - Validation Logic using `FluentValidation`.
   - **Dependencies**: Depends strictly on the Domain Layer.

3. **`CarMarketApp.Infrastructure` (Outer Implementation)**
   - **Purpose**: Implements interfaces defined in the Application layer and connects the app to external technical systems.
   - **Contents**: 
     - **Persistence**: `Entity Framework Core` DbContext, Configurations, and Migrations for SQL Server.
     - **Identity**: `.NET Core Identity` setup for seamless user management.
     - **Mapping**: Advanced object mapping logic powered by `AutoMapper`.
   - **Dependencies**: Depends on Application and Domain layers.

4. **`CarMarketApp.WebAPI` (Presentation)**
   - **Purpose**: The entry point of the system exposing HTTP endpoints.
   - **Contents**: WEB API Controllers (`AdvertsController`, `BrandsController`, `ModelsController`, `UsersController`), Dependency Injection Registration, Serilog Configurations, swagger generation, and etc.
   - **Dependencies**: Depends on Application and Infrastructure layers.

---

## 🛠 Tech Stack

- **Platform**: `.NET 8`
- **API Architecture**: `RESTful` pattern via ASP.NET Core Web API
- **Database**: `SQL Server`
- **ORM Framework**: `Entity Framework Core 8` (Code-First Approach)
- **Authentication**: `JWT (JSON Web Tokens)` with Access and Refresh Token rotation methodology.
- **Identity Provider**: `ASP.NET Core Identity Framework`
- **Messaging / Dispatching**: `MediatR`
- **Validation**: `FluentValidation`
- **Logging Management**: `Serilog`
- **API Documentation**: `Swagger / Swashbuckle`
- **Mapping**: `AutoMapper`

---

## 🔐 Advanced Security & Authentication

The `UsersController` implements an advanced authentication system:
- **Registration & Login**: Native Identity flows.
- **JWT Authorization**: Requests secured using Bearer tokens.
- **Role-Based Access Control (RBAC)**: Certain endpoints (e.g. `CreateBrand`, `DeleteBrand`) enforce constraints like `[Authorize(Roles = "Admin,Moderator")]`.
- **Refresh Token Generation**: Handles access token expirations by providing seamless token-refresh mechanics natively.

---

## ⚡ Main API Features & Endpoints Structure

The controllers follow the `api/[controller]/[action]` routing mechanism. 

### 🏷 Brands (`BrandsController`)
- `POST /api/Brands/Create` - Creates a new car brand (Admin/Moderator).
- `PUT /api/Brands/Update` - Edits existing brand details.
- `GET /api/Brands/GetAll` - Retrieves a paginated list of brands using `BrandFilterDto`.
- `DELETE /api/Brands/Delete` - Soft delete a brand.
- `PUT /api/Brands/Restore` - Restores a deleted brand.

### 🚘 Models (`ModelsController`)
- Provides similar CRUD operations linked functionally to the Parent `Brand`.

### 📰 Adverts (`AdvertsController`)
- Core logic for sellers to list their cars.
- Endpoints for querying adverts using diverse filters (e.g., price ranges, years, mileage).

---

## 🚀 Getting Started (Local Development)

### 1️⃣ Prerequisites
To build and run this project, ensure you have installed:
- [.NET 8.0 SDK]
- [SQL Server]

### 2️⃣ Clone the Repository
```bash
git clone https://https://github.com/feridhub8/CleanArchitecture.CarMarketApp
cd CarMarketApp/src/CarMarketApp.WebAPI
