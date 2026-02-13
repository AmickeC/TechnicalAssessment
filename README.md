# Technical Assessment

This solution demonstrates a simple User Management system built with ASP.NET Core, following a clean separation between API and UI layers.

## Solution Structure

The solution is split into two main projects:
1. TechnicalAssessment.API
Implements the backend RESTful Web API using ASP.NET Core.
Responsibilities:
* Handles business logic
* Manages database access
* Exposes HTTP endpoints
Main Components:
* Models
  Defines domain entities:
	User
	Group
	Permission
* DTO (Data Transfer Object)
Used to control API input/output and prevent direct exposure of database entities.
* Controllers
Handle API endpoints such as:
	Create user
	Update user
	Delete user
	Retrieve users and groups
* Data/TechnicalAssessmentContext.cs
Configures Entity Framework Core and database access.
*Swagger
Enabled for API testing and documentation.

2. TechnicalAssessment.UI
ASP.NET Core MVC frontend application that consumes the API.

## Responsibilities:
* Provides user interface
* Performs CRUD operations via HTTP calls to the API
* Displays aggregated user and group information

## Key Components:
* Controllers
	UsersController – communicates with the API via HttpClient
* Views
	Views/Users/Index.cshtml – Displays user list and statistics
	Views/Users/Create.cshtml
	Views/Users/Edit.cshtml
*wwwroot
	Contains static front-end assets (CSS, JS, etc.)


## Key Technical Decisions

1. ASP.NET Core Web API as development framework
2. Entity Framework Core used as the ORM to interface with the database
3. DTOs used to improve maintainability - used to define types of objects passed to APIs
4. Separate API and UI - makes the two components more independent and reduces risk of database exposure since no direct access to database from UI
5. Email validation implemented for creating and updating users to prevent duplicate email entries in the database.


## How to run the project

The project needs to be run in 2 separate terminals - one for UI and one for API. 

Prerequisites:
* Make sure you have .NET version 8 installed, this can be done by https://dotnet.microsoft.com/en-us/download/dotnet/8.0

Steps:
* Open the TechnicalAssessment.API.csproj file and update the `<TargetFramework>` tag to use .NET 8:
`<TargetFramework>net8.0</TargetFramework>`
* Open two terminal windows and run these commands

#### Terminal 1:

`cd TechnicalAssessment.API`

`dotnet run`

This exposes the API at http://locahost:5244/api.

Swagger UI is available at http://localhost:5244/swagger


#### Terminal 2:

`cd TechnicalAssessment.UI`

`dotnet run`

This exposes the UI at http://localhost:5204.

## Notes

Features not included due to limited time
1. Unit tests
