# SupportFlow

SupportFlow is a full-stack customer support ticket management system.

The application allows customers to create support tickets, support agents to manage assigned tickets, and admins to control users, categories, assignments, and the overall ticket process.

## Project Description

SupportFlow is designed to simulate a real-world support desk system.

Customers can create support tickets for technical problems, billing questions, account issues, bug reports, feature requests, and general questions.

Support agents can view assigned tickets, reply to customers, and update ticket statuses.

Admins can manage all tickets, assign tickets to support agents, manage users, manage categories, and monitor the overall support workflow.

## Tech Stack

### Backend

* C#
* ASP.NET Core Web API
* Entity Framework Core
* PostgreSQL
* JWT Authentication
* Role-based Authorization
* Swagger

### Frontend

* React
* JavaScript
* Tailwind CSS
* Redux Toolkit
* React Router
* Axios

### Tools

* VS Code
* Git
* GitHub
* Insomnia
* PostgreSQL

## User Roles

The application includes three main user roles:

* Customer
* SupportAgent
* Admin

## Core Features

* User authentication
* Role-based authorization
* Ticket creation
* Ticket listing
* Ticket detail view
* Ticket message system
* Ticket status management
* Ticket priority management
* Ticket assignment
* Category management
* User management
* Dashboard pages

## Ticket Statuses

* Open
* In Progress
* Waiting for Customer
* Resolved
* Closed

## Ticket Priorities

* Low
* Medium
* High
* Critical

## Ticket Categories

* Technical Issue
* Billing
* Account
* Bug Report
* Feature Request
* General Question

## Database Draft

Main database tables:

* Users
* Tickets
* TicketMessages
* TicketCategories
* TicketStatusHistories
* TicketAttachments
* Notifications

## Current API Endpoints

### Database

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/database/health` | Checks the database connection |

### Categories

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/categories` | Returns all categories |
| GET | `/api/categories/{id}` | Returns a category by ID |
| POST | `/api/categories` | Creates a category |
| PUT | `/api/categories/{id}` | Updates a category |
| DELETE | `/api/categories/{id}` | Deletes or deactivates a category |

### Tickets

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/tickets` | Returns all tickets |
| GET | `/api/tickets/{id}` | Returns ticket details |
| POST | `/api/tickets` | Creates a new ticket |
| PUT | `/api/tickets/{id}` | Updates a ticket |
| DELETE | `/api/tickets/{id}` | Deletes a ticket |

## Backend Structure

```text
SupportFlow.Api
├── Controllers
├── Data
│   └── Configurations
├── DTOs
│   ├── Categories
│   └── Tickets
├── Helpers
├── Interfaces
├── Middleware
├── Migrations
├── Models
├── Repositories
└── Services

## Current Limitations

- Authentication has not been implemented yet.
- Customer IDs are temporarily supplied in ticket creation requests.
- Ticket assignment and status update endpoints are not available yet.
- Ticket messages, notifications and file attachments are planned for later phases.
- Authorization rules will be added after JWT authentication.

## Project Status

The first month of backend development has been completed.

Completed:

- Project planning and feature definition
- Database design
- ASP.NET Core Web API setup
- PostgreSQL and Entity Framework Core configuration
- Initial database migration
- Category CRUD operations
- Ticket CRUD operations
- DTO and service layer structure
- Swagger and Insomnia API testing

