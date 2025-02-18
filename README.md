# Store Codex Backend

A comprehensive e-commerce backend solution developed during Route IT Training Center's latest cycle, built with .NET Core and clean architecture principles.

## 📋 Project Overview

This backend project demonstrates the implementation of modern e-commerce functionalities using .NET Core, focusing on scalability, maintainability, and industry best practices.

## 🌟 Key Features

### Core Functionalities
- **Product Management**
  - Advanced Filtering & Sorting
  - Product Categories & Brands
  - Image Upload & Management
  - Pagination Support

- **User Management**
  - JWT Authentication
  - Role-based Authorization
  - User Profile Management

- **Shopping Features**
  - Shopping Basket Operations
  - Order Processing
  - Multiple Delivery Options
  - Stripe Payment Integration

- **Performance & Error Handling**
  - Redis Caching
  - Global Error Handling
  - Custom Exception Management
  - Request/Response Logging

### Technical Implementation
- **Architecture & Patterns**
  - Clean Architecture
  - Specification Pattern
  - Generic Repository Pattern
  - Unit of Work Pattern

- **Database**
  - Entity Framework Core
  - SQL Server
  - Code-First Migrations
  - Seed Data

## 🔧 Technical Stack

- **.NET 6.0**
- **Entity Framework Core**
- **SQL Server**
- **Redis**
- **JWT Authentication**
- **Stripe Payment API**
- **AutoMapper**
- **Swagger/OpenAPI**

## 📁 Project Structure
```
Store.Codex/
├── Dtos/                  # Data Transfer Objects
├── Entities/              # Domain Entities
├── Interfaces/            # Contracts & Interfaces
├── Specifications/        # Query Specifications
├── Helpers/              # Utility Classes
└── Services/             # Business Logic
```

## 🚀 Getting Started

### Prerequisites
- .NET 6.0 SDK 
- SQL Server
- Redis Server
- Stripe Account (for payments)

### Installation Steps

1. Clone the repository
```bash
git clone [Your Repository URL]
```

2. Configure your appsettings.json:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your SQL Server Connection String",
    "Redis": "Your Redis Connection String"
  },
  "Token": {
    "Key": "your secret key",
    "Issuer": "your issuer"
  },
  "StripeSettings": {
    "PublishableKey": "your stripe publishable key",
    "SecretKey": "your stripe secret key"
  }
}
```

3. Run migrations
```bash
dotnet ef database update
```

4. Start the application
```bash
dotnet run
```

## 📚 API Documentation

Complete API documentation is available via Swagger at `/swagger` endpoint when running the application.

### Postman Collection
A comprehensive Postman collection is included in the `/Postman` directory, containing all API endpoints for testing.

## 🧪 Testing

```bash
dotnet test
```

## 🎓 Acknowledgments

Special thanks to Route IT Training Center for the guidance and support during the development of this project.
