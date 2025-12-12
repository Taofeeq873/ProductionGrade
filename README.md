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

#Tech Stack

.NET 8 with C#

Entity Framework Core (PostgreSQL)

MediatR for CQRS pattern (Commands & Queries)

FluentValidation for input validation

Mapster for object mapping

RabbitMQ for message queuing

An email service provider(Brevo) for mailing

Sentry for error tracking

Swagger / OpenAPI for API documentation

PostgreSQL for relational database


#Assumptions

Database is PostgreSQL, running on configured port.

Email notifications are processed asynchronously via RabbitMQ queues for scalability and reliability.

Email service requires API key and endpoint configured.

Product stock quantity and status are the source of truth for availability.

#Setup Instructions
#Prerequisites

.NET 9 SDK

PostgreSQL

RabbitMQ

An email service provider(Brevo) for mailing

  **Configuration:**  
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
       "ApiKey": "your_emailprovider_api_key"
     }
   }
