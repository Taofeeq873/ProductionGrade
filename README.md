# ProductionGrade
#Overview

ProductionGrade is a modern, scalable e-commerce API built using .NET 8, implementing Domain-Driven Design (DDD) principles and CQRS via MediatR. It manages products and orders with concurrency-safe inventory control and robust error handling.

#Features

Manage products with stock tracking and status updates.

Create orders with multiple product selections.

Concurrency-safe inventory decrement to prevent overselling.

Asynchronous command and query handling via MediatR.

RESTful API with versioning and swagger UI.

Centralized validation and error handling.

Configuration via environment variables and appsettings.json.

# Jekapass API

## Tech Stack

- **Backend Framework:** ASP.NET Core Web API  
- **ORM & Database:** Entity Framework Core with PostgreSQL  
- **Mediation & CQRS:** MediatR  
- **Validation:** FluentValidation  
- **Mapping:** Mapster  
- **Logging:** Microsoft.Extensions.Logging, with optional Sentry integration  
- **API Documentation:** Swagger (Swashbuckle)

---

## Setup Instructions

1. **Prerequisites:**  
   - .NET 9 SDK
   - PostgreSQL (ensure running locally or remote)  

2. **Configuration:**  
   Provide these values in `appsettings.json` or environment variables:

   ```json
   {
      "ConnectionStrings": {
    "DefaultConnection": ""
     },
     "DB_HOST": "localhost",
     "DB_PORT": 5432,
     "DB_NAME": "ProductionGrade_db",
     "DB_USERNAME": "postgres",
     "DB_PASSWORD": "your_password",
   }


#Assumptions

The product inventory manages availability through domain logic ensuring safe stock decrement.

API input/output use JSON with camelCase naming conventions.

Product stock quantity and status are the source of truth for availability.


#Project Architecture

**Domain:** Core business logic and entities (Product, Order, etc.), custom exceptions, and configurations.

**Application:** CQRS commands and queries, business logic handlers using MediatR.

**Infrastructure:** Database context, repositories, external service implementations.

**Api:** Presentation layer with controllers, middleware, filters, and DI setu
