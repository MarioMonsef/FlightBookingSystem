# ✈️ Flight Booking System API

A backend web application developed using **ASP.NET Core 8** that powers a flight booking system. It handles user management, seat booking, payment processing, and real-time updates using SignalR.

---

## 🏗️ Architecture

This project follows the **Three-Layer Architecture**:

- **Presentation Layer**: ASP.NET Core Web API  
- **Application Layer**: Services, DTOs, and business logic  
- **Data Access Layer**: Repository pattern using EF Core & Unit of Work  

---

## 🔐 Authentication & Authorization

- Integrated with **ASP.NET Core Identity**  
- Supports **JWT authentication** for secure access  
- Role-based access control (e.g., Manager, User)  

---

## 💺 Seat Booking

- Users can view and book seats for flights  
- Real-time seat updates using **SignalR**  
- Frontend clients are notified of booking status using the `SeatUpdated` event  

---

## 💳 Payment Integration

- Integrated with **Stripe API**  
- Supports:  
  - Creating payment intents  
  - Confirming payments  
  - Handling refunds  

---

## ⚙️ Middleware

- Global exception handling middleware to standardize API error responses  
- Rate limiting middleware to restrict excessive requests (max 8 requests per 30 seconds per IP)  

---

## 🔒 Security

- Enforced HTTPS redirection for secure communication  
- Security headers added via middleware (e.g., `X-Content-Type-Options`, `X-XSS-Protection`, `X-Frame-Options`)  

---

## 🛠️ Setup & Configuration

- Configure JWT settings in `appsettings.json` (issuer, audience, secret key)  
- Setup database connection string for EF Core  

---

## 🚀 Running the Project

- Run locally with `.NET 8 SDK` installed  
- Requires SQL Server instance  
- Stripe API keys needed for payment integration  

---


## 📡 Technologies Used

- `ASP.NET Core 8`  
- `Entity Framework Core`  
- `SignalR`  
- `Stripe`  
- `AutoMapper`  
- `SQL Server`  

---

## 📬 Future Enhancements

- Frontend integration using Angular 🌐  
- Admin panel to manage flights and seats ✈️  
- Notification system via email or SMS 🔔  

---

## 🧑‍💻 Author

**Mario** – [Your GitHub Profile](https://github.com/MarioMonsef)

---

## 📄 License

This project is licensed under the **Apache License 2.0** – see the [LICENSE](LICENSE) file for details.
