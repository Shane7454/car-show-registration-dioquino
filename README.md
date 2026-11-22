# Car Show Registration System 2027

A web-based Car Show Registration System developed for **AutoVista Events – Car Show 2027**, taking place at Sorsogon City Coliseum.

The system allows participants to register their vehicles as Exhibitors, Competitors, or VIPs. It provides event staff with registration records and payment information through a centralized application.

## Team Members

| Member | Role |
|---|---|
| Shane Raven Dioquino | Backend Developer |
| [Partner's Full Name] | Frontend Developer |

## Project Structure

```text
Car-Show-Registration/
│
├── Backend/             # C# .NET Web API
│
├── Frontend/            # React application
│
├── Docs/
│   └── screenshots/     # Project documentation screenshots
│
├── .gitignore
└── README.md
```

## Technologies Used

### Backend
- C#
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- FluentValidation
- Scalar

### Frontend
- React JS
- Vite
- Tailwind CSS / Bootstrap

### Development Tools
- Visual Studio 2026
- Visual Studio Code
- Git
- GitHub
- DB Browser for SQLite

## Backend Features

- Participant registration
- Automatic registration number generation
- Registration fee calculation
- Payment status tracking
- Duplicate plate number prevention
- Registration filtering and pagination
- Registration details lookup
- Dashboard summary and statistics
- SQLite database persistence

## How to Run the Backend

1. Open the solution in Visual Studio 2026.
2. Restore the NuGet packages.
3. Set the API project as the startup project.
4. Build the solution.
5. Run the application.

Alternatively, using the terminal:

```bash
cd Backend
dotnet restore
dotnet build
dotnet run --project Api
```

Scalar API documentation:

```text
http://localhost:5205/scalar/v1
```

Use the actual configured API port if it differs.

## How to Run the Frontend

Open another terminal:

```bash
cd Frontend
npm install
npm run dev
```

Frontend development URL:

```text
http://localhost:5173
```

## Database

The backend uses SQLite with Entity Framework Core.

Database file:

```text
carshow.db
```

The database stores participant registration information and supports persistent data storage.

## GitHub Repository

Repository: https://github.com/Shane7454/car-show-registration-dioquino

Backend development branch:

```text
feature/backend-development
```

## Project Documentation

Project screenshots and supporting documentation are stored in:

```text
Docs/screenshots/
```

---

**Course:** Integrative Programming 2  
**Project:** Car Show Registration System – Backend Task  
**School:** Computer Communication Development Institute (CCDI), Sorsogon Campus  
**Instructor:** Elvin Manuel R. Luces, MIT  
**Academic Year:** 2026
