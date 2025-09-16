Visitor Management System (ASP.NET Core MVC + EF Core)

A modern Visitor Management System built with ASP.NET Core MVC and Entity Framework Core, designed to replace legacy VB6-based solutions.
It helps organizations manage daily visitors efficiently with features like auto-entry, exit tracking, and instant PDF pass generation.

🚀 Features

Visitor Entry Form – Responsive (works on desktop, tablet, and mobile).

Auto-fill Visitor Info – Mobile number lookup auto-populates visitor details from previous visits.

In/Out Tracking –

Entry time (InTime) and date are auto-recorded.

Visitors can be marked OUT with one click (updates OutTime).

Daily Visitor List – Shows only today’s records, keeping the list clean and fast.

PDF Slip Generation (A6) – Instantly generates a visitor pass (ready for small thermal printers).

Responsive UI – Tables on desktop, card view on mobile for smooth UX.

SQL Server Backend – Secure storage with Entity Framework Core.

🛠 Tech Stack

ASP.NET Core MVC 8

Entity Framework Core (SQL Server)

Bootstrap 5 (responsive UI)

QuestPDF (PDF slip generation)

📸 Screenshots

(Add your screenshots here — e.g. Visitor Form, Visitor List, PDF Slip)

⚡ Installation & Setup

Clone the repo

git clone https://github.com/your-username/Visitor-Management.git
cd Visitor-Management


Update the database connection string
In appsettings.json, configure your SQL Server connection:

"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=VisitorDB;Trusted_Connection=True;TrustServerCertificate=True;"
}


Apply migrations (if using EF Code First)

dotnet ef database update


Run the application

dotnet run


Open in browser:

https://localhost:7184

📄 License

This project is licensed under the MIT License – see the LICENSE
 file for details.