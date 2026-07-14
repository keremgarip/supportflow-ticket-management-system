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

## Project Status

Currently in design phase.

Week 2 focus:

* Database and table creation
* ERD/database relations
* Entity list
* Table fields
* Relation logic

