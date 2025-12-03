# Firmeza.web

**Firmeza.web** is a comprehensive web application solution designed for e-commerce and business management. It features a robust architecture combining a server-side rendered MVC admin panel, a modern React-based client storefront, and a RESTful API backend, all powered by ASP.NET Core 8 and PostgreSQL.

## 🚀 Project Overview

The solution is divided into three main components:

1.  **Firmezaa.web (Admin Panel)**: An ASP.NET Core MVC application for administrative tasks, product management, and reporting.
2.  **Web.Api (Backend API)**: A RESTful API that serves data to the client application and handles business logic.
3.  **Cliente (Storefront)**: A modern, responsive React application for customers to browse products and make purchases.

## 🛠️ Tech Stack

### Backend (Firmezaa.web & Web.Api)
-   **Framework**: ASP.NET Core 8.0
-   **Language**: C#
-   **Database**: PostgreSQL (via Entity Framework Core)
-   **ORM**: Entity Framework Core 8
-   **Authentication**: ASP.NET Core Identity & JWT (JSON Web Tokens)
-   **Documentation**: Swagger / OpenAPI
-   **Tools**:
    -   **EPPlus**: For Excel data import/export.
    -   **QuestPDF**: For generating PDF reports.
    -   **AutoMapper**: For object-to-object mapping.

### Frontend (Cliente)
-   **Framework**: React 19
-   **Build Tool**: Vite
-   **Language**: JavaScript / JSX
-   **Routing**: React Router DOM
-   **HTTP Client**: Axios
-   **Styling**: CSS Modules / Standard CSS

### Infrastructure
-   **Containerization**: Docker & Docker Compose
-   **Database**: PostgreSQL 15 Alpine

## 📦 Prerequisites

Ensure you have the following installed:
-   [Docker Desktop](https://www.docker.com/products/docker-desktop) (recommended)
-   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (for local development)
-   [Node.js 18+](https://nodejs.org/) (for local development)

## 🐳 Quick Start with Docker

The easiest way to run the entire application is using Docker Compose.

1.  **Clone the repository**:
    ```bash
    git clone https://github.com/Santy1924/Firmeza.web.git
    cd Firmeza.web
    ```

2.  **Start the services**:
    ```bash
    docker compose up --build -d
    ```

3.  **Access the applications**:
    -   **Client Storefront**: [http://localhost:5176](http://localhost:5176)
    -   **Admin Panel (MVC)**: [http://localhost:5280](http://localhost:5280)
    -   **API Swagger**: [http://localhost:5275/swagger](http://localhost:5275/swagger)
    -   **PostgreSQL**: Port 5435

    *Note: The Docker configuration maps ports as follows to avoid conflicts:*
    *   *API: 5275*
    *   *Client: 5176*
    *   *MVC Admin: 5280*
    *   *PostgreSQL: 5435*

## 🔧 Local Development Setup

If you prefer to run services individually:

### 1. Database (PostgreSQL)
Ensure you have a PostgreSQL instance running. Update the connection strings in `appsettings.Development.json` in both `Firmezaa.web` and `Web.Api` projects.

### 2. Web API
```bash
cd Web.Api
dotnet restore
dotnet run --launch-profile http
```
*Runs on: http://localhost:5272*

### 3. Admin Panel (MVC)
```bash
cd Firmezaa.web
dotnet restore
dotnet run
```
*Runs on: http://localhost:5013*

### 4. Client (React)
```bash
cd Cliente
npm install
npm run dev
```
*Runs on: http://localhost:5173*

## 🔑 Key Features

-   **User Management**: Secure registration and login with role-based access control (Admin, Client).
-   **Product Catalog**: Browse, search, and filter products.
-   **Shopping Cart**: Add items, view summary, and checkout.
-   **Admin Dashboard**: Manage products, users, and view sales reports.
-   **Data Import/Export**: Import products via Excel.
-   **PDF Generation**: Generate invoices and reports.

## 📂 Project Structure

```
Firmeza.web/
├── Cliente/                # React Frontend
│   ├── src/
│   ├── public/
│   └── Dockerfile
├── Firmezaa.web/           # ASP.NET Core MVC Admin
│   ├── Controllers/
│   ├── Models/
│   ├── Views/
│   └── appsettings.json
├── Web.Api/                # ASP.NET Core Web API
│   ├── Controllers/
│   ├── Services/
│   └── Dockerfile
├── Firmeza.Tests/          # Unit & Integration Tests
│   ├── Services/
│   └── Controllers/
└── docker-compose.yml      # Docker orchestration
```

## 🤝 Contributing

1.  Fork the repository.
2.  Create a feature branch (`git checkout -b feature/AmazingFeature`).
3.  Commit your changes (`git commit -m 'Add some AmazingFeature'`).
4.  Push to the branch (`git push origin feature/AmazingFeature`).
5.  Open a Pull Request.

## 👤 Author

**Santy1924**
-   GitHub: [@Santy1924](https://github.com/Santy1924)

## 📄 License

Distributed under the MIT License. See `LICENSE` for more information.
