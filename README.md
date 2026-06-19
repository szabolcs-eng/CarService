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

*(Insert your screenshots here! Example: <img src="./assets/dashboard.png" width="800">)*
* **Login and Registration**
* **User Dashboard**
* **Vehicle Details & Average Consumption**

## 👨‍💻 About the Developer

I am a 4th-semester Computer Science Engineering student at Óbuda University. Highly motivated, goal-oriented, and passionate about modern web technologies. I am actively looking for a **software/web development internship** where I can contribute to real-world projects and learn from experienced professionals.

If you find my code promising and think I'd be a good fit for your team, please feel free to reach out!

---
*Built with enthusiasm and a lot of coffee ☕ in 2026.*
