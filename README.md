# Reactivities: Activity Management Platform (ASP.NET Core + React)

Reactivities is a full‑stack activity management platform built  on **ASP.NET Core and React**. The project demonstrates a clean architecture–based backend, a modern React client, and real‑time features using SignalR. It is designed to showcase production‑ready patterns such as CQRS, MediatR, Cookie-based authentication using API.NET Core Identity, and scalable client‑side state management.

---

## 🚀 Live Demo

**Deployed Application:** https://reactivities-hj.azurewebsites.net

> The application is deployed on Azure App Service and demonstrates the full functionality of the platform, including authentication, activity management, real-time chat, and profile features.
- **Note:** Email verification is limited due to Resend API constraints on free-tier accounts. For the best experience, please use GitHub OAuth to sign in.

---

## 🌟 Key Features

### Backend (ASP.NET Core API)

- **Activity Management (CRUD):** Create, view, edit, and delete activities with support for attendees and hosts.
- **CQRS with MediatR:** Commands and queries are separated for better maintainability and testability.
- **Authentication & Authorization:**
  - Cookie‑based authentication using ASP.NET Core Identity
  - External login support via **GitHub OAuth**
- **User Profiles:**
  - Profile viewing and editing
  - Follow / unfollow users
  - Profile photo upload and management
- **Real‑time Communication:**
  - SignalR hub for live activity chat
  - Real‑time comment updates without page refresh
- **Validation & Error Handling:**
  - FluentValidation for request validation
  - Centralized exception handling via custom middleware
- **Pagination & Filtering:**
  - Cursor‑based pagination for activities
  - Filtering by date, host, and attendance
- **Email Integration:**
  - Email confirmation and notifications using **Resend**
- **Security:**
  - Cookies handling
  - Refresh cookies
  - Role‑based authorization
- **CORS Configuration:**
  - Properly configured for React client consumption

---

### Frontend (React Client)

- **Modern React Stack:**
  - React + TypeScript
  - Vite for fast development builds
- **UI & Styling:**
  - Material UI (MUI)
  - Responsive layout
- **State Management:**
  - React Context for global state
  - React Query (TanStack Query) for server‑state management
- **Forms & Validation:**
  - React Hook Form
  - Zod for schema‑based validation
- **Routing & Navigation:**
  - React Router
  - Protected routes for authenticated users
- **Real‑time Updates:**
  - SignalR client integration for live chat
- **Authentication Handling:**
  - Login, register, email confirmation, password reset flows

---

## 📦 Getting Started

### Prerequisites

- .NET SDK
- Node.js (LTS recommended)
- Docker (optional, for SQL Server)
- SQL Server (if not using Docker)

---

### Installation

1. **Clone the repository**

```bash
git clone https://github.com/Hassanj34/Reactivities.git
cd Reactivities
```

2. **Start SQL Server (Docker – recommended)**

```bash
docker-compose up -d
```

Or configure a local SQL Server instance and update:

```
API/appsettings.Development.json
```

3. **Apply database migrations**

```bash
dotnet ef database update --project Persistence --startup-project API
```

4. **Configure secrets**

Add the following using **user secrets** or `appsettings.Development.json`:

- GitHub OAuth ClientId & ClientSecret
- Resend API key
- Cloudinary API key, API secret & Cloud Name (For images upload)

---

### Running the Application

#### Backend

```bash
cd API
dotnet run
```

API will run on `https://localhost:5001` (or configured port).

#### Frontend

```bash
cd client
npm install
npm run dev
```

Client will be available at `http://localhost:3000` (Vite default).

---

## 📂 Project Structure

```
Reactivities/
├── API/                          # ASP.NET Core Web API
│   ├── Controllers/
│   ├── Middleware/
│   ├── SignalR/
│   ├── DTOs/
│   ├── Extensions/
│   ├── Services/
│   ├── Program.cs
│   └── appsettings*.json
│
├── Application/                  # Application layer (CQRS)
│   ├── Activities/
│   ├── Profiles/
│   ├── Core/
│   ├── Interfaces/
│   └── Behaviors/
│
├── Domain/                       # Domain entities
│   ├── Activity.cs
│   ├── AppUser.cs
│   └── Photo.cs
│
├── Infrastructure/               # External services & implementations
│   ├── Email/
│   ├── Photos/
│   ├── Security/
│   └── Persistence helpers
│
├── Persistence/                  # EF Core data access
│   ├── AppDbContext.cs
│   ├── Migrations/
│   └── Seed/
│
├── client/                       # React application
│   ├── public/
│   └── src/
│       ├── app/
│       │   ├── api/              # Axios configuration
│       │   ├── layout/
│       │   ├── models/
│       │   ├── router/
│       │   ├── stores/
│       │   └── common/
│       ├── features/
│       │   ├── activities/
│       │   ├── profiles/
│       │   ├── users/
│       │   └── comments/
│       ├── main.tsx
│       └── App.tsx
│
├── docker-compose.yml
└── README.md
```

---

## 🎓 Learning Outcomes

This project demonstrates:

- Clean Architecture in ASP.NET Core
- CQRS with MediatR
- Secure authentication and authorization
- Real‑time communication with SignalR
- Production‑style React application structure
- Full‑stack integration between .NET and React

---

## 📄 License

This project is for educational and portfolio purposes.

