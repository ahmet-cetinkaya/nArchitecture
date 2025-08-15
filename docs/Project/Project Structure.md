[`🏠`](../README.md) > [`Project`](./README.md) > `Project Structure`

# 📂 Project Structure

This document provides an overview of the project structure and the different layers within the NArchitecture framework. Understanding the structure will help you navigate the codebase and contribute effectively.

NArchitecture is inspired by Clean Architecture, which emphasizes separation of concerns and promotes a scalable and maintainable codebase. This architecture allows for independent development, testability, and easy adaptation to changing requirements or technologies.

## 🏗️ Folder Structure

The project is organized into the following main folders, reflecting the architectural layers of the application:

```
NArchitecture.Starter/
├── core/
│   ├── Application/
│   └── Domain/
├── infrastructure/
│   └── Persistence/
└── presentation/
    └── WebApi/
```

## 🏗️ Architectural Layers

### 1. **Core**

The Core folder contains the central parts of the application. This is the heart of the software system and houses the domain models, entities, and application services.

#### 1.1 **Domain Layer**

The Domain Layer forms the foundation of the application, containing the entity definitions and domain models that represent the core concepts of the business domain. This layer is completely isolated from external concerns and focuses on defining the structure and relationships of business objects.

- **Entities**: Represent the core business objects with identity and lifecycle (e.g., User, Product, Order). These define the structure and properties of primary domain concepts.
- **Value Objects**: Immutable objects defined by their attributes rather than identity (e.g., Address, Money, DateRange). They are used to encapsulate domain concepts that don't need identity tracking.
- **Aggregates**: Clusters of domain objects that can be treated as a single unit, with one entity serving as the aggregate root. They define transactional consistency boundaries within the domain.
- **Domain Models**: Define the structure and relationships of objects within the domain without necessarily containing complex business logic.

#### 1.2 **Application Layer**

The Application Layer serves as the primary container for business logic and rules. It orchestrates and coordinates the domain objects to perform specific application tasks and implements the core business functionality. This layer defines the jobs the software is supposed to do and contains the use cases of the application.

- **Use Cases**: Specific business operations (commands and queries) that implement the business logic. Each use case typically represents one specific action a user can perform.
- **Business Logic**: The core rules, validations, and workflows that define how the application should behave.
- **DTOs (Data Transfer Objects)**: Simplified data structures for transferring data between layers, particularly from the application layer to the presentation layer.
- **Events**: Domain events that represent significant changes or occurrences within the domain and can trigger side effects or additional processing.
- **Services**: Application services that implement business logic by coordinating between domain objects and infrastructure services.
- **Repositories Interfaces**: Contracts defining how to access and persist domain objects without specifying the actual implementation details.
- **Validators**: Components that ensure incoming data meets required business rules before processing.
- **Mappers**: Objects responsible for transforming data between different representations across layer boundaries using Mapster for high-performance object mapping.

### 2. **Infrastructure**

The Infrastructure folder contains implementations for interfaces defined in the core layers and provides technical capabilities to the system. This layer handles external concerns like databases, file systems, third-party APIs, and other infrastructure-related aspects that support the inner layers.

#### 2.1 **Persistence Layer**

The Persistence Layer is responsible for data persistence and retrieval. It implements the repository interfaces defined in the Application Layer and handles all database-related operations.

- **Repositories**: Concrete implementations of the repository interfaces that handle data access operations and transform between domain objects and database entities.
- **Data Access**: Database contexts, configurations, and connection management that provide a bridge between the domain model and the database schema.
- **Migrations**: Database versioning and schema evolution tools that manage the database structure changes over time.
- **Query Objects**: Specialized classes for complex or optimized data access operations.
- **ORM Configurations**: Object-Relational Mapping configurations that define how domain objects map to database tables and columns.

### 3. **Presentation**

The Presentation Layer is the entry point to the application, handling user interactions and formatting responses. It depends on the Application Layer to execute use cases and present the results in an appropriate format. This layer adapts to the different delivery mechanisms without affecting the core application logic.

#### 3.1 **WebApi**

The WebApi is a specific implementation of the Presentation Layer that exposes the application functionality through HTTP endpoints.

- **Controllers**: API endpoints that handle HTTP requests, validate inputs, invoke the appropriate use cases, and format HTTP responses.
- **Middleware**: Request processing components that handle cross-cutting concerns like authentication, logging, error handling, and request/response transformation.
- **Configuration**: Application settings, dependency injection setup, and service registrations that configure the web service.
- **Filters**: Components that intercept the request/response pipeline to implement cross-cutting concerns like exception handling, action filtering, and result formatting.
- **API Documentation**: Swagger/OpenAPI specifications and configurations that provide interactive documentation for the API endpoints.
