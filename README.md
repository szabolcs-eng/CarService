# 🚗 CarService - Vehicle & Maintenance Tracker

A comprehensive Full-Stack web application designed to help car owners track their vehicle expenses, fuel logs, and maintenance history in one clean, transparent platform.

This project demonstrates the seamless integration of a modern, type-safe client-side interface (React + TypeScript) with a robust, scalable backend (ASP.NET Core REST API).

## ✨ Key Features

* 🔐 **Secure Authentication:** User registration and login featuring password hashing (BCrypt) and JWT (JSON Web Token) based session management.
* 🚘 **Vehicle Management:** Register and track multiple user-specific vehicles (by Brand, Model, License Plate, and Year).
* ⛽ **Fuel Logs & Statistics:** Log refueling events with dates, quantities, and costs. The system features a built-in algorithm that automatically calculates the vehicle's **average consumption (L/100km)** based on logged odometer readings.
* 🔧 **Service History:** Keep a precise, documented record of maintenance, repairs, and associated costs.
* 📱 **Responsive UI:** A clean, fast, and mobile-friendly user experience powered by Bootstrap 5.

## 🛠️ Tech Stack

The project is built on modern industry standards, maintaining a strict separation of concerns between the frontend and backend layers:

**Frontend (Client-Side):**
* **Framework:** React (Powered by Vite for maximum performance)
* **Language:** TypeScript (Ensuring strict type safety and reducing runtime errors)
* **Routing & Network:** React Router DOM (SPA navigation), Axios (HTTP API requests)
* **Styling:** Bootstrap 5, Custom CSS

**Backend (Server-Side):**
* **Framework:** C# / .NET (ASP.NET Core Web API)
* **Database & ORM:** SQL Server / SQLite, Entity Framework Core (Code-First approach)
* **Security:** JWT Bearer Token authentication, BCrypt password encryption, CORS policies

## 🚀 Getting Started (Local Development)

To test the project locally, clone the repository and run both the backend and frontend servers by following these steps:

### 1. Running the Backend
1. Navigate to the backend directory (via Visual Studio or terminal).
2. Run the database migrations in the Package Manager Console: `Update-Database`, or via CLI: `dotnet ef database update`.
3. Start the server: `dotnet run`. The server will run on `https://localhost:7196` by default.

### 2. Running the Frontend
1. Open a new terminal and navigate to the frontend folder.
2. Install the required dependencies:
   `npm install`
3. Start the Vite development server:
   `npm run dev`
4. Open your browser and navigate to the provided local URL (usually `http://localhost:5173/`).

## 📸 Screenshots

* **Login and Registration**
<img width="1873" height="918" alt="Képernyőkép 2026-06-19 143553" src="https://github.com/user-attachments/assets/276d3336-b0c7-461f-9b5f-568133004eb3" />

<img width="1867" height="922" alt="Képernyőkép 2026-06-19 143621" src="https://github.com/user-attachments/assets/d6bb15d3-f23d-4a7a-adb1-516d79977d8c" />

* **User Dashboard**
<img width="1871" height="925" alt="Képernyőkép 2026-06-19 143702" src="https://github.com/user-attachments/assets/236c3dde-00f9-4476-bfdf-3dd7bd659f1d" />

* **Vehicle Details & Average Consumption**
<img width="1877" height="915" alt="Képernyőkép 2026-06-19 143907" src="https://github.com/user-attachments/assets/0afdb9ab-5c5a-4f51-bac9-856b625c73a4" />
