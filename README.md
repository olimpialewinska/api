# Task API

This repository contains a C# .NET project that provides a RESTful API for task management. The API supports authentication and authorization using tokens and allows users to perform various operations such as retrieving a list of tasks, fetching individual tasks assigned to a user, adding, deleting, and editing tasks. The project utilizes an in-memory database.

# Features
- Authentication and Authorization: The API implements token-based authentication and authorization to secure the endpoints.
- Task Management: Users can perform CRUD (Create, Read, Update, Delete) operations on tasks.

# Technologies Used
- C# .NET: The project is implemented using the C# programming language and the .NET framework.
- In-Memory Database: An in-memory database is utilized to store and manage task data.
- RESTful API: The API follows the principles of Representational State Transfer (REST) architecture.

# Installation
To set up the project locally, follow these steps:

```bash
git clone https://github.com/olimpialewinska/api.git
cd api
dotnet restore
dotnet build
```

# API Endpoints
The following API endpoints are available:

**Authentication:**

- POST /api/Auth/register: Register a new user.
- POST /api/Auth/login: Login and obtain an authentication token.

**Task Management:**

- GET /api/Tasks/{userId}: Retrieve a user's list of all tasks.
- POST /api/Tasks/{userId}: Add a new task.
- GET /api/Tasks/{userId}/completed: Retrieve a list of completed tasks.
- GET /api/Tasks/{userId}/incomplete: Retrieve a list of incomplete tasks.
- GET /api/Tasks/{userId}/{taskId}: Retrieve an individual task by its ID.
- DELETE /api/Tasks/{userId}/{taskId}: Delete a task by its ID.
- POST /api/Tasks/{userId}/{taskId}/description: Update the description of a task.
- POST /api/Tasks/{userId}/{taskId}/completed: Mark a task as completed.


# Authentication and Authorization
To authenticate and authorize API requests, follow these steps:
1. Register a user account.
2. Obtain an authentication token by sending a POST request to the /api/auth/login endpoint with valid credentials.
3. Include the token in the Authorization header of subsequent requests as a bearer token.
