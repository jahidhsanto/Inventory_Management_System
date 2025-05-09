# 🧾 Inventory Management System

![Full-Stack Project](https://img.shields.io/badge/Type-Full--Stack-blueviolet)
![Made with ASP.NET](https://img.shields.io/badge/Made%20With-ASP.NET-blue)
![Database-SQLServer](https://img.shields.io/badge/Database-SQL_Server-lightgrey)
![Status](https://img.shields.io/badge/Status-In_Progress-yellow)

## 📌 Overview

The Inventory Management System is a full-stack web application designed to manage products, materials, stock levels, and user roles. Built using **ASP.NET Web Forms (.aspx)**, **C#**, and **SQL Server**, this system supports role-based access, authentication, and inventory updates through triggers and stored procedures.

## 🚀 Features

- 🔐 User Authentication & Role-based Access
- 📦 Product and Material Management
- 📉 Stock Level Updates with SQL Triggers
- 📁 Stored Procedures for Data Operations
- 📊 Dashboard Views for Admin & User Roles
- 📎 SQL Scripts for Easy Setup

## 🧰 Tech Stack

| Layer     | Technology                              |
| --------- | --------------------------------------- |
| Front-End | ASP.NET Web Forms (.aspx), HTML, CSS    |
| Back-End  | C#, ADO.NET                             |
| Database  | SQL Server, Stored Procedures, Triggers |
| Auth      | Custom login, Role-based UI access      |

## 🏗️ Project Structure
```
Inventory_Management_System/
├── App_Code/ # Business logic files (C#)
├── Pages/ (.aspx) # Front-end UI pages
├── App_Code/ (Business logic)
├── SQL Scripts/ # DB create, reset, bulk insert
├── Web.config # App configuration
├── Inventory_Management.sln # Solution file
└── README.md # This file
```

## 🖥️ Screenshots

## 📂 Setup Instructions

1. **Clone the Repository**
   ```bash
   git clone https://github.com/jahidhsanto/Inventory_Management_System.git
   ```
2. **Import the SQL Scripts**
   Create a database in SQL Server.
   Run 1. Database Create.sql,
   then 2. Reset Tables.sql,
   and 3. BULK Insert data.sql.
4. **Open in Visual Studio**
   Open Inventory_Management.sln.
   Update Web.config with your DB connection string.

5. **Run the Application**
   Press F5 in Visual Studio or deploy to IIS/localhost.

## 🔐 Default Login Credentials (for demo)

| Role  | Username | Password |
| ----- | -------- | -------- |
| Admin | admin    | admin123 |
| User  | user     | user123  |

(Change these in DB or add seeding if needed.)

## ✍️ Author

Jahid Hasan Santo
📧 [Your Email]
🔗 LinkedIn
💻 Portfolio/GitHub
