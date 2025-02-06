# DocTime - Appointment Management API

## Overview
DocTime is a RESTful API built with **.NET 8** for managing patient appointments in a healthcare clinic. It includes user authentication, appointment scheduling, and doctor management features.

## Features
- **User Authentication** (Register & Login with JWT Token)
- **CRUD Operations for Appointments**
- **Doctor Creation**
- **JWT-based Authorization**
- **Error Handling & Input Validation**
- **Entity Framework Core with MS SQL Database**

## Technology Stack
- **.NET 8** (ASP.NET Core)
- **Entity Framework Core**
- **MS SQL Server**
- **JWT Authentication**

## Prerequisites
- Install **.NET 8 SDK**
- Install **MS SQL Server**

## Setup Instructions
```sh
# Clone the repository
git clone https://github.com/yourusername/DocTime.git
cd DocTime

# Restore dependencies
dotnet restore

# Update appsettings.json with your DB connection string

# Apply database migrations
dotnet ef database update

# Run the project
dotnet run
```

## API Endpoints
### Authentication
#### Register a User
```http
POST /register
```
**Request Body:**
```json
{
  "username": "testuser",
  "password": "securepassword"
}
```
#### Login
```http
POST /login
```
**Response:** Returns JWT Token

### Appointment Management (Requires Authentication)
#### Create an Appointment
```http
POST /appointments
```
**Request Body:**
```json
{
  "patientName": "John Doe",
  "patientContact": "123-456-7890",
  "appointmentDateTime": "2025-01-12T10:00:00",
  "doctorId": 1
}
```

#### Get All Appointments
```http
GET /appointments
```
#### Get Appointment by ID
```http
GET /appointments/{id}
```
#### Update an Appointment
```http
PUT /appointments/{id}
```
#### Delete an Appointment
```http
DELETE /appointments/{id}
```

### Doctor Management (Requires Authentication)
#### Create a Doctor
```http
POST /doctors
```
**Request Body:**
```json
{
  "doctorName": "Dr. Smith"
}
```

## Authentication & Authorization
- JWT Token must be included in the `Authorization` header for all protected endpoints.
```http
Authorization: Bearer YOUR_TOKEN_HERE
```

## Error Handling
- Returns **400 Bad Request** for invalid data.
- Returns **401 Unauthorized** for missing/invalid token.
- Returns **404 Not Found** if the resource does not exist.

## Bonus Features
- **Password Hashing** using ASP.NET Identity
- **Validation** ensures appointments are scheduled in the future
- **Automated Tests** (Optional)

## License
This project is open-source and available under the [MIT License](LICENSE).

---

Feel free to contribute or report issues!
