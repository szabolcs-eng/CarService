# 🚗 CarService - Vehicle & Maintenance Tracker

A comprehensive Full-Stack web application designed to help car owners track their vehicle expenses, fuel logs, and maintenance history in one clean, transparent platform.

This project demonstrates the seamless integration of a modern, type-safe client-side interface (React + TypeScript) with a robust, scalable backend (ASP.NET Core REST API), fully containerized and deployed to the cloud.

## 🌐 Try it out! (Live Demo)
You can easily test the application without downloading or installing anything. Just click the links below:

* **Frontend Application (User Interface):** [https://carserviceapp-6w30.onrender.com](https://carserviceapp-6w30.onrender.com)
* **Backend API (Swagger UI):** [https://carserviceapi-app.onrender.com/swagger](https://carserviceapi-app.onrender.com/swagger)

> **⚠️ Cloud Hosting Note:** This project is hosted on Render's free tier. If the application hasn't been used for 15 minutes, the backend will go to sleep. **The first login or request might take ~50 seconds while the server wakes up.** Additionally, the current SQLite database is reset upon server restart, so test data may be cleared periodically.

## ✨ Key Features

* 🔐 **Secure Authentication:** User registration and login featuring password hashing and JWT (JSON Web Token) based session management.
* 🚘 **Vehicle Management:** Register and track multiple user-specific vehicles (by Brand, Model, License Plate, and Year).
* ⛽ **Fuel Logs & Statistics:** Log refueling events with dates, quantities, and costs. The system features a built-in algorithm that automatically calculates the vehicle's **average consumption (L/100km)** based on logged odometer readings.
* 🛡️ **Advanced Data Validation:** Robust backend validation using **FluentValidation** to ensure data integrity before it reaches the database.
* 📄 **Pagination:** Efficient data handling with backend pagination for lists and logs, parsed seamlessly by the frontend.
* 📱 **Responsive UI:** A clean, fast, and mobile-friendly user experience powered by Bootstrap 5.

## 🛠️ Tech Stack

The project is built on modern industry standards, maintaining a strict **Clean Architecture** and separation of concerns:

**Frontend (Client-Side):**
* **Framework:** React (Powered by Vite for maximum performance)
* **Language:** TypeScript (Ensuring strict type safety)
* **Routing & Network:** React Router DOM (SPA navigation), **Axios with Interceptors** (for automatic JWT injection)
* **Styling:** Bootstrap 5, Custom CSS

**Backend (Server-Side):**
* **Framework:** C# / .NET 10 (ASP.NET Core Web API)
* **Architecture:** Clean Architecture principles
* **Database & ORM:** SQLite, Entity Framework Core (Code-First approach)
* **Security & Validation:** JWT Bearer Token, FluentValidation, CORS policies

**DevOps & Infrastructure:**
* **Containerization:** Docker (Multi-stage builds)
* **Web Server:** Nginx (Configured for React SPA routing fallback)
* **CI/CD & Hosting:** Continuous Deployment via Render Web Services

## 📸 Screenshots

* **Login and Registration**
<img width="1873" height="918" alt="Képernyőkép 2026-06-19 143553" src="https://github.com/user-attachments/assets/276d3336-b0c7-461f-9b5f-568133004eb3" />

<img width="1867" height="922" alt="Képernyőkép 2026-06-19 143621" src="https://github.com/user-attachments/assets/d6bb15d3-f23d-4a7a-adb1-516d79977d8c" />

* **User Dashboard**
<img width="1871" height="925" alt="Képernyőkép 2026-06-19 143702" src="https://github.com/user-attachments/assets/236c3dde-00f9-4476-bfdf-3dd7bd659f1d" />

* **Vehicle Details & Average Consumption**
<img width="1877" height="915" alt="Képernyőkép 2026-06-19 143907" src="https://github.com/user-attachments/assets/0afdb9ab-5c5a-4f51-bac9-856b625c73a4" />
