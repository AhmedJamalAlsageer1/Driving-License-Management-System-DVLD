# Driving & Vehicle License Management System (DVLD)

### C# | Windows Forms | SQL Server | ADO.NET | 3-Tier Architecture

## Overview

DVLD is a desktop application developed using C#, Windows Forms, and SQL Server to manage driving license services and related administrative operations.

The system supports driver management, license applications, local and international licenses, and various licensing transactions.

This project was implemented by me from scratch as a hands-on software development project, based on the educational DVLD project concept and requirements presented by Mohammed Abu Hadhoud.

The primary goal was to strengthen my practical skills in application architecture, business logic, database operations, and maintainable code organization.

## Key Features

### People and Driver Management
- Manage personal information and driver records.
- Search for and retrieve existing records.
- Display driver information and license history.

### Local Driving Licenses
- Manage local driving license applications.
- Issue new driving licenses.
- Renew existing licenses.
- Replace lost or damaged licenses.
- View local license information.

### International Driving Licenses
- Issue international driving licenses.
- View international license information and history.

### License Detention
- Detain driving licenses.
- Release detained licenses.
- Manage related administrative operations.

### Application Management
- Manage license-related applications.
- Handle application requirements and business rules.

## Technologies Used

| Technology | Purpose |
|---|---|
| C# | Application development |
| Windows Forms | Desktop user interface |
| SQL Server | Relational database |
| ADO.NET | Database connectivity and operations |
| .NET | Application platform |
| Git & GitHub | Version control and source code hosting |

## Project Architecture

The project separates application responsibilities into multiple layers and supporting components.

### Presentation Layer — `DVLD`
Contains the Windows Forms user interface and handles user interaction.

### Business Logic Layer — `DVLDBisnessLayer`
Contains application rules, validations, and business operations.

### Data Access Layer — `DVLDDataAccessLayer`
Handles database communication and data persistence.

### Data Transfer Objects — `DVLD.DTOs`
Contains DTO classes used to transfer structured data between application components.

### Shared Layer — `SharedLayer`
Contains shared functionality used across the application.

This separation helps improve code organization, maintainability, and separation of concerns.

## Database Operations

The application uses SQL Server and ADO.NET for database access.

Related database operations can be grouped into transactions to help maintain data consistency when an operation involves multiple changes.

## Screenshots

Screenshots of the application's main interfaces will be added here.

## Project Demonstration

A full walkthrough video demonstrating the application's functionality will be linked here.

## My Contribution

I implemented the application code and organized the project structure myself, using the educational DVLD project as the foundation for its concept and functional requirements.

My work involved implementing the user interface, application logic, database access, DTOs, and supporting components.

The project also served as an opportunity to practice architectural separation and the implementation of real application workflows.

## Educational Acknowledgment

The DVLD project concept and educational requirements were introduced by **Mohammed Abu Hadhoud** through Programming Advices.

This repository represents my own implementation of the project.

Credit for the original educational concept and requirements belongs to their respective creator.

## Author

**Ahmed Jamal**

GitHub: [AhmedJamalAlsageer1](https://github.com/AhmedJamalAlsageer1)

## Project Status

Functional educational desktop application, developed for practical learning and portfolio demonstration.
