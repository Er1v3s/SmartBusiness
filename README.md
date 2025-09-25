# SmartBusiness - Business Management System with AI-Powered Sales Forecasting

**SmartBusiness** is a modern, scalable web application designed to streamline business operations, including user authentication, product and service management, transaction processing, and predictive analytics. Built with a **microservices architecture**, it leverages technologies like **React**, **.NET**, **Python**, **MSSQL**, **MongoDB**, **Redis**, and **RabbitMQ** to ensure modularity, performance, and an intuitive user experience.

---

## Table of Contents

- [SmartBusiness - Business Management System with AI-Powered Sales Forecasting](#smartbusiness---business-management-system-with-ai-powered-sales-forecasting)
  - [Table of Contents](#table-of-contents)
  - [Project Overview](#project-overview)
  - [Implementation Details](#implementation-details)
    - [System Architecture](#system-architecture)
    - [Graphical User Interface](#graphical-user-interface)
    - [API Gateway](#api-gateway)
    - [Account Service](#account-service)
    - [Sales Service](#sales-service)
    - [Transaction Write Service](#transaction-write-service)
    - [Transaction Read Service](#transaction-read-service)
    - [Machine Learning Service](#machine-learning-service)
    - [Security and Authorization](#security-and-authorization)
    - [Data Validation](#data-validation)
    - [Databases](#databases)
    - [Data Synchronization](#data-synchronization)
    - [Optimization](#optimization)
    - [Design Patterns and Best Practices](#design-patterns-and-best-practices)
    - [System Testing](#system-testing)
    - [Application Monitoring](#application-monitoring)
  - [Screenshots](#screenshots)
  - [Getting Started](#getting-started)
    - [Prerequisites](#prerequisites)
    - [Installation](#installation)
    - [Running](#running)
  - [License](#license)

---

## Project Overview

This project delivers a robust platform for managing business operations with a focus on scalability, security, and predictive analytics. It features a responsive **React** front-end, a modular **microservices** back-end, and advanced data processing using machine learning. The system is designed to handle user management, product/service tracking, transaction processing, and financial forecasting, making it suitable for businesses seeking efficient and data-driven solutions.

---

## Implementation Details

### System Architecture

The system is built on a **microservices architecture**, dividing functionality into independent services for scalability and maintainability. It comprises three layers:
- **Presentation Layer**: A **React**-based front-end for user interaction.
- **Integration Layer**: An **API Gateway** (using **YARP** in **ASP.NET Core**) for routing requests.
- **Business Logic Layer**: Microservices in **.NET** (C#) and **Python**, handling specific business functions.

Data is managed using **MSSQL** for relational data, **MongoDB** for transactions, and **Redis** for caching. **RabbitMQ** facilitates asynchronous communication between services.

> ![Architecture Diagram](./docs/arch.png)
> **Diagram**: System architecture overview

### Graphical User Interface

The front-end is developed with **React**, **TypeScript**, and **Vite**, styled with **TailwindCSS** for a responsive and visually appealing interface. Key features include:
- Reusable components (e.g., forms, tables, modals, pagination).
- Support for **light** and **dark** modes, toggleable by users.
- Smooth animations and navigation effects for enhanced **UX**.
- Hybrid rendering with **React Router** for seamless single-page application (SPA) navigation.


### API Gateway

The **API Gateway**, built with **YARP** in **ASP.NET Core**, serves as the entry point for all client requests. It:
- Routes HTTP requests to appropriate microservices based on predefined rules.
- Provides **health check** endpoints to monitor the gateway and microservices.
- Ensures secure and centralized communication.

> ![API Gateway Flow](./docs/helthcheck_v3.png)
> **Diagram**: API Gateway data flow

### Account Service

The **Account Service** (**.NET**, **MSSQL**) manages user authentication, authorization, and company data. It:
- Handles registration, login, password reset, and token management (**JWT**, **Refresh Tokens**).
- Stores sensitive data in a secure **AccountDb** database.
- Embeds company context in **JWT** tokens for access control.

### Sales Service

The **Sales Service** (**.NET**, **MSSQL**) manages **CRUD** operations for products and services in a dedicated **SalesDb**. It:
- Stores product/service details (e.g., name, price, tax).
- Provides data for transaction processing and visualization.
- Deletes associated data upon company deletion via **RabbitMQ** events.

### Transaction Write Service

The **Transaction Write Service** (**.NET**, **MongoDB**) handles transaction creation, updates, and deletions. It:
- Uses **MongoDB** (**TransactionWriteDb**) for fast write operations.
- Publishes events (**TransactionCreatedEvent**, etc.) to **RabbitMQ** for synchronization.
- Ensures data integrity during company deletion.

### Transaction Read Service

The **Transaction Read Service** (**.NET**, **MongoDB**) is optimized for reading transaction data. It:
- Uses **MongoDB** (**TransactionReadDb**) for efficient data retrieval.
- Implements **Redis** caching to reduce database load.
- Supports filtering for transaction history and statistics.

### Machine Learning Service

The **Machine Learning Service** (**Python**, **FastAPI**) provides predictive analytics using the **Prophet** model. It:
- Analyzes historical transaction data from **TransactionReadDb**.
- Generates forecasts for sales, taxes, net revenue, and individual service sales.
- Ensures data quality through cleaning and validation with **Pandas**.

### Security and Authorization

Security is a priority, implemented through:
- **JWT** for authentication, validated for every request.
- **Refresh Tokens** stored as **HttpOnlyCookies** for session management.
- **PBKDF2** password hashing for secure storage.
- **HTTPS** for encrypted communication.
- **CORS** to restrict access to trusted domains.

### Data Validation

Data integrity is ensured through dual-layer validation:
- **Front-end**: Real-time validation using HTML and JavaScript.
- **Back-end**: **FluentValidation** in **.NET** services to enforce data rules and protect against malicious inputs.

### Databases

The system uses:
- **MSSQL**: **AccountDb** for user/company data, **SalesDb** for products/services.
- **MongoDB**: **TransactionWriteDb** for writes, **TransactionReadDb** for reads.
- **Redis**: Caching frequently accessed data for performance.

### Data Synchronization

Asynchronous synchronization via **RabbitMQ** ensures data consistency:
- **Transaction Write/Read**: Events like **TransactionCreatedEvent** sync data between **TransactionWriteDb** and **TransactionReadDb**.
- **Global Sync**: **CompanyDeletedEvent** triggers data cleanup across all services.

### Optimization

Performance is optimized using:
- **Redis** caching for frequent queries, invalidated by **RabbitMQ** events.
- Separation of read/write operations to reduce database load.
- Efficient data models in **MongoDB** for high-throughput operations.

### Design Patterns and Best Practices

The system adheres to:
- **Clean Architecture** for modular code.
- **CQRS** with **MediatR** for command/query separation.
- **Repository Pattern** for data access abstraction.
- **SOLID** principles for maintainable code.
- **Git** with **GitHub Actions** for CI/CD.

### System Testing

Testing ensures reliability:
- **Unit Tests**: 244 scenarios using **xUnit**, **Moq**, and **FluentAssertions** for validation logic.
- **Manual Tests**: Front-end and API testing with **Postman** and **Swagger**.
- **Integration Tests**: Verify microservice communication and authorization.

### Application Monitoring

Comprehensive monitoring is achieved with:
- **OpenTelemetry**: Collects telemetry data from microservices.
- **Prometheus**: Aggregates metrics for analysis.
- **Grafana**: Visualizes performance dashboards.
- **Jaeger**: Tracks distributed request flows.


Grafana Dashboard: <div><img src="./docs/apple-macbookpro14-mockup-grafana&jaeger/grafana-front.png" alt="Opis" width="960" /></div>
Jaeger Trace: <div><img src="./docs/apple-macbookpro14-mockup-grafana&jaeger/jaeger-front.png" alt="Opis" width="960" /></div>

---

## Screenshots

<!-- IPHONE -->
<div>
    <img src="./docs/apple-iphone-15-pro-black-titanium-mockup/home_1-portrait.png" alt="Opis" height="720" style="" />
    <img src="./docs/apple-iphone-15-pro-black-titanium-mockup/home_2-portrait.png" alt="Opis" height="720" />
</div>

<div>
    <img src="./docs/apple-iphone-15-pro-black-titanium-mockup/login-portrait.png" alt="Opis" height="720" />
    <img src="./docs/apple-iphone-15-pro-black-titanium-mockup/register-portrait.png" alt="Opis" height="720" />
</div>

<div>
    <img src="./docs/apple-iphone-15-pro-black-titanium-mockup/forgot-password-portrait.png" alt="Opis" height="720" />
    <img src="./docs/apple-iphone-15-pro-black-titanium-mockup/reset-password-portrait.png" alt="Opis" height="720" />
</div>


<!-- MAC -->
<div>
    <img src="./docs/apple-macbookpro14-mockup/dashboard_laptop-front.png" alt="Opis" width="960" />
</div>

<div>
    <img src="./docs/apple-macbookpro14-mockup/register_sale_laptop-front.png" alt="Opis" width="960" />
</div>

<div>
    <img src="./docs/apple-macbookpro14-mockup/services_laptop-front.png" alt="Opis" width="960" />
</div>

<div>
    <img src="./docs/apple-macbookpro14-mockup/services_edit_laptop-front.png" alt="Opis" width="960" />
</div>

<div>
    <img src="./docs/apple-macbookpro14-mockup/transactions_laptop-front.png" alt="Opis" width="960" />
</div>

<div>
    <img src="./docs/apple-macbookpro14-mockup/statystyki_laptop-front.png" alt="Opis" width="960" />
</div>

<div>
    <img src="./docs/apple-macbookpro14-mockup/predykcja_laptop-front.png" alt="Opis" width="960" />
</div>

<!-- IPAD -->
<div>
    <img src="./docs/apple-ipadpro11-spacegrey-mockup/dashboard_tablet-landscape.png" alt="Opis" width="720" />
</div>

<div>
    <img src="./docs/apple-ipadpro11-spacegrey-mockup/kalendarz_tablet-landscape.png" alt="Opis" width="720" />
</div>

<div>
    <img src="./docs/apple-ipadpro11-spacegrey-mockup/statystyki_tablet-landscape.png" alt="Opis" width="720" />
</div>

<div>
    <img src="./docs/apple-ipadpro11-spacegrey-mockup/predykcja_tablet-landscape.png" alt="Opis" width="720" />
</div>

<div>
    <img src="./docs/apple-ipadpro11-spacegrey-mockup/delete_company_tablet-landscape.png" alt="Opis" width="720" />
</div>

<div>
    <img src="./docs/apple-ipadpro11-spacegrey-mockup/change_password_tablet-landscape.png" alt="Opis" width="720" />
</div>

## Getting Started

### Prerequisites
- **Node.js** (v22+)
- **.NET SDK** (v9+)
- **Python** (v3+)
- **MSSQL**, **MongoDB**, **Redis**, **RabbitMQ**

### Installation
> TODO
<!-- 1. Clone the repository:
   ```bash
   git clone <repository-url>
   ```
2. Install front-end dependencies:
   ```bash
   cd frontend && npm install
   ```
3. Configure environment variables for databases, APIs, and RabbitMQ.
4. Start microservices and the API Gateway as per their documentation. -->

### Running
> TODO
<!-- 1. Launch the front-end:
4. Start microservices and the API Gateway as per their documentation.

### Running
1. Launch the front-end:
   ```bash
   npm run dev
   ```
2. Start each microservice and the API Gateway.
3. Access the application at `http://localhost:3000`. -->


## License

This project is licensed under the **[MIT License](LICENSE)**.