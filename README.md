# 📦 Inventory Management System

![Full-Stack Project](https://img.shields.io/badge/Type-Full--Stack-blueviolet)
![Made with ASP.NET](https://img.shields.io/badge/Made%20With-ASP.NET-blue)
![Database](https://img.shields.io/badge/Database-SQL%20Server-greenlight)
![Status](https://img.shields.io/badge/Status-In_Progress-yellow)

A full-stack Inventory Management System built with **ASP.NET Web Forms**, **C#**, and **SQL Server**. It enables efficient inventory tracking, user management, stock operations, and custom reporting—all integrated through a layered architecture and secured with role-based access.

---

## 🚀 Features

- 🔐 **User Authentication** – Secure login with role-based access control
- 📦 **Inventory Tracking** – Manage items, categories, and stock quantities
- 🔄 **Stock Management** – Add, update, or remove stock entries
- 📊 **Reporting** – Generate inventory reports and summaries on inventory data
- 🧠 **Stored Procedures & Triggers** – Logic encapsulated in SQL for better performance and reuse

---

## 🛠️ Technologies Used

| Layer     | Technology                              |
| --------- | --------------------------------------- |
| Front-End | ASP.NET Web Forms (.aspx), HTML, CSS    |
| Back-End  | C#, ADO.NET                             |
| Database  | SQL Server, Stored Procedures, Triggers |
| Auth      | Custom login, Role-based UI access      |

---

## 📂 Project Structure

```bash
Inventory_Management_System/
│
├── SQL Scripts/
│   ├── 1. Database Create.sql
│   ├── 2. Reset Tables.sql
│   ├── 3. BULK Insert data.sql
│   ├── 4. Update Serial Number status.sql
│   ├── 5. Insert Opening Balance in Ledger.sql
│   ├── Procedures/
│   │   ├── Insert Users.sql
│   │   ├── Login.sql
│   │   ├── Password Reset.sql
│   │   ├── Update User Role.sql
│   │   ├── Delete User.sql
│   ├── Triggers/
│   │   └── Update Stock_Quantity.sql
│
├── Project Directory
│   ├── App_Code/                   # Business logic in C#
│   ├── Pages/                      # ASPX front-end UI pages
│   ├── Web.config                  # App configuration file
│   ├── Inventory_Management.sln    # Visual Studio solution
│   └── README.md                   # This file
```

## 🖥️ Screenshots


## ⚙️ Getting Started
✅ **Prerequisites**
- SQL Server (2016+ recommended)
- SQL Server Management Studio (SSMS)
- Visual Studio with ASP.NET & Web Development workload

🧪 **Installation Steps**
1. **Clone the Repository**

```bash
git clone https://github.com/jahidhsanto/Inventory_Management_System.git
cd Inventory_Management_System
```

2. **Set Up Database**
- Open SSMS.
- Run the following SQL scripts in this order:
   - `Database Create.sql`
   - `Reset Tables.sql`
   - `BULK Insert data.sql`
   - `Update Serial Number status.sql`
   - `Insert Opening Balance in Ledger.sql` 

3. **Add Stored Procedures and Triggers**
- Execute all SQL files inside:
```bash
/SQL Scripts/Procedures/
/SQL Scripts/Triggers/
```

4. **Configure the Application**
Open `Inventory_Management.sln` in Visual Studio.

Update the connection string in `Web.config` to match your SQL Server instance.

5. **Run the Application**
Press **F5** or run with **IIS Express** in Visual Studio.

## 💻 Usage
Once installed, you can:

- Log in with your inserted users
- Add or remove items from stock
- Update inventory status
- Run reports using custom SQL queries

## 🙌 Contributing
Contributions, feature requests, and suggestions are welcome!
Steps to contribute:
1. Fork the repository
2. Create a feature branch (`git checkout -b feature-name`)
3. Commit your changes
4. Push to your fork
5. Create a Pull Request ✅

## 📬 Contact

Feel free to reach out if you have any questions or suggestions:

- 👤 **Author**: Md. Jahid Hassan  
- 📧 **Email**: [jahidhsanto@gmail.com](mailto:jahidhsanto@gmail.com)  
- 💻 **GitHub**: [@jahidhsanto](https://github.com/jahidhsanto)  
- 🔗 **LinkedIn**: [linkedin.com/in/jahidhsanto](https://www.linkedin.com/in/jahidhsanto/)  
