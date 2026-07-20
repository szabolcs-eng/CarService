# 🚗 CarService - Vehicle & Maintenance Tracker

A comprehensive Full-Stack web application designed to help car owners track their vehicle expenses, fuel logs, and maintenance history in one clean, transparent platform.

This project demonstrates the seamless integration of a modern, type-safe client-side interface with a robust, scalable backend, fully automated with CI/CD pipelines and deployed to the Microsoft Azure cloud.

## 🌐 Try it out!
You can easily test the application without downloading or installing anything. Just click the links below:

* **Frontend Application (User Interface):** [https://your-frontend-link.azurewebsites.net](https://carservice-app-szabolcs-eng-hu-hsc9bkhub3gvgahz.polandcentral-01.azurewebsites.net)
* **Backend API (Swagger UI):** [https://your-backend-link.azurewebsites.net/swagger](https://carservice-api-hu-e8bnhmf9h9g2gwg8.polandcentral-01.azurewebsites.net/swagger)

> **☁️ Enterprise Cloud Hosting:** Unlike basic hobby projects hosted on sleeping free tiers, this application is deployed on **Microsoft Azure Linux App Services** with **Always On** enabled. Both the .NET 10 API and the Node/Vite SPA frontend deliver instant, zero-latency responses 24/7, powered by continuous deployment workflows.

## ✨ Key Features

* 🔐 **Secure Authentication:** User registration and login featuring BCrypt password hashing and JWT (JSON Web Token) based session management.
* 🚘 **Vehicle Management:** Register and track multiple user-specific vehicles by storing their Brand, Model, License Plate, and Year.
* ⛽ **Fuel Logs & Statistics:** Log refueling events with dates, quantities, and costs, while the system automatically calculates the vehicle's exact **average consumption (L/100km)** based on odometer progression.
* 🛠️ **Service & Maintenance Tracking:** Log detailed maintenance history, including service descriptions, exact costs, and odometer readings at the time of repair.
* 🛡️ **Advanced Data Validation:** Robust backend validation using **FluentValidation** to guarantee strict data integrity before any database transaction occurs.
* ⚡ **Modern & Responsive UI:** A lightning-fast, mobile-first user experience styled from the ground up using **Tailwind CSS v4**, providing a sleek and highly customizable visual interface.

## 🛠️ Tech Stack & Architecture

The project is built on industry-standard technologies, strictly adhering to **Clean Architecture** principles and separation of concerns:

**Frontend:**
* **Framework:** React (Powered by Vite for blazing-fast build times and HMR)
* **Language:** TypeScript (Ensuring end-to-end type safety)
* **Styling:** **Tailwind CSS v4** (Utility-first, modern responsive design)
* **Routing & Network:** React Router DOM (SPA navigation), **Axios with Interceptors** (for automatic JWT Bearer token injection)

**Backend:**
* **Framework:** C# / .NET 10 (ASP.NET Core Web API)
* **Architecture:** Clean Architecture principles with DTO pattern
* **Database & ORM:** SQLite, Entity Framework Core (Code-First approach)
* **Security & Auth:** JWT Bearer Token validation, BCrypt password hashing, and custom CORS policies

**DevOps & Cloud Infrastructure:**
* **Cloud Provider:** Microsoft Azure (Linux App Services for both Backend API and Frontend UI)
* **CI/CD Automation:** **GitHub Actions** (Automated build, test, and continuous deployment workflows triggered on main branch commits)
* **Containerization & Process Management:** Docker multi-stage builds, PM2 / Nginx configured for seamless React Single Page Application (SPA) routing fallback

## 📸 Screenshots

* **Login and Registration**
<img width="1866" height="924" alt="Képernyőkép 2026-07-16 084038" src="https://github.com/user-attachments/assets/3bfb0a4d-1189-42d1-b0ad-8d58214617dd" />

<img width="1862" height="920" alt="Képernyőkép 2026-07-16 084134" src="https://github.com/user-attachments/assets/22e222f3-9624-4041-8422-e70c15458b3d" />

* **User Dashboard**
<img width="1870" height="921" alt="Képernyőkép 2026-07-16 084527" src="https://github.com/user-attachments/assets/32365a97-2b8a-421f-ac4c-e6a39ccb45ac" />

* **Vehicle Details & Average Consumption**
<img width="1872" height="898" alt="Képernyőkép 2026-07-16 084749" src="https://github.com/user-attachments/assets/a082086a-d6e9-4647-b419-989e034bef66" />
