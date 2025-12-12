# ProductionGrade
#Overview

ProductionGrade is a modern, scalable e-commerce API built using .NET 8, implementing Domain-Driven Design (DDD) principles and CQRS via MediatR. It manages products and orders with concurrency-safe inventory control, messaging integration, and robust error handling.

#Features

Manage products with stock tracking and status updates.

Create orders with multiple product selections.

Concurrency-safe inventory decrement to prevent overselling.

Asynchronous command and query handling via MediatR.

RESTful API with versioning and swagger UI.

Centralized validation and error handling.

Integration with mailing service and RabbitMQ message broker.

Configuration via environment variables and appsettings.json.

# Jekapass API

## Tech Stack

- **Backend Framework:** ASP.NET Core Web API  
- **ORM & Database:** Entity Framework Core with PostgreSQL  
- **Mediation & CQRS:** MediatR  
- **Messaging Queue:** RabbitMQ (for async email handling)  
- **Email Provider:** Brevo (via transactional email API)  
- **Validation:** FluentValidation  
- **Mapping:** Mapster  
- **Logging:** Microsoft.Extensions.Logging, with optional Sentry integration  
- **API Documentation:** Swagger (Swashbuckle) with JWT security support  

---

## Setup Instructions

1. **Prerequisites:**  
   - .NET 8 SDK
   - PostgreSQL (ensure running locally or remote)  
   - RabbitMQ server (default config expects localhost:5672)  
   - email provider

2. **Configuration:**  
   Provide these values in `appsettings.json` or environment variables:

   ```json
   {
     "DB_HOST": "localhost",
     "DB_PORT": 5432,
     "DB_NAME": "ProductionGrade_db",
     "DB_USERNAME": "postgres",
     "DB_PASSWORD": "your_password",
     "MessageBroker": {
       "HostName": "localhost",
       "Port": 5672,
       "UserName": "guest",
       "Password": "guest"
     },
     "MailConfiguration": {
       "FromName": "ProductionGrade",
       "FromEmail": "email",
       "ApiKey": "your_api_key"
     }
   }


#Assumptions

The product inventory manages availability through domain logic ensuring safe stock decrement.

Email notifications are processed asynchronously via RabbitMQ queues for scalability and reliability.

The RabbitMQ queue named emailQueue is durable and persistent.

API input/output use JSON with camelCase naming conventions.

JWT authentication is expected (Swagger is configured for Bearer token auth).

The Brevo API key is valid and configured in the environment to enable transactional email sending.
