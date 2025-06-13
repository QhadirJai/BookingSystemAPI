# Room Booking API

A modern ASP.NET Core 8.0 Web API for managing room bookings, users, and roles (Staff and Customer).  
This project uses minimal APIs, JWT authentication, and Entity Framework Core with SQL Server.

## Features

- User registration and login (JWT-based authentication)
- Role-based authorization (Staff, Customer)
- Room management and booking endpoints
- Swagger/OpenAPI documentation
- Modular, minimal API endpoint structure
- Database seeding for roles

## Technologies

- ASP.NET Core 8.0
- Entity Framework Core 8 (SQL Server)
- ASP.NET Core Identity
- JWT Bearer Authentication
- Swashbuckle (Swagger/OpenAPI)

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server (local or remote)

### Setup

1. **Clone the repository:**
   
2. **Configure the database:**
   - Update the `DefaultConnection` string in `appsettings.json` or `appsettings.Development.json` to point to your SQL Server instance.

3. **Configure JWT settings:**
   - Set the `JwtSettings` section in `appsettings.json`:
     
4. **Run database migrations (if needed):**
   
5. **Run the application:**
   
6. **Access Swagger UI:**
   - Navigate to `https://localhost:5215/` (or your configured port) for interactive API docs.

## API Overview

### Authentication

- `POST /api/auth/login`  
  Login and receive a JWT token.

- `POST /api/auth/register`  
  Register a new customer.

### Staff Management

- `POST /api/admin/staff`  
  Register a new staff member.  
  **Requires authentication.**  
  Example request body:
  
### Room & Booking Endpoints

- `GET /api/rooms`  
  List all rooms.

- `POST /api/bookings`  
  Create a new booking.

*(See Swagger UI for full endpoint documentation and request/response schemas.)*

## Roles

- **Staff**: Can be registered by authenticated users.
- **Customer**: Can self-register.

## Development Notes

- The admin role is disabled; only Staff and Customer roles are supported.
- JWT tokens must be included in the `Authorization: Bearer <token>` header for protected endpoints.

## Contributing

Contributions are welcome! Please open issues or submit pull requests.

## License

This project is licensed under the MIT License.
