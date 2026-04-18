# C# Scheduling Application
### Developed by Benjamin Weiglein

A business desktop application for scheduling and tracking client appointments, built with C# and .NET Framework with MySQL database integration.

---

## Table of Contents
- [Requirements](#requirements)
- [Database Setup](#database-setup)
- [Connection Settings](#connection-settings)
- [Running the Application](#running-the-application)
- [Database Schema](#database-schema)

---

## Requirements

| Tool | Version |
|---|---|
| Visual Studio | 2019 or later |
| .NET Framework | 4.8 |
| MySQL Server | 8.0 |
| MySql.Data (NuGet) | 9.6.0 |

---

## Database Setup

1. Install **MySQL Server 8.0** and open **MySQL Workbench**
2. Open and run the included `database_setup.sql` script
3. The script will automatically:
   - Create the `client_schedule` database
   - Create all required tables
   - Populate the database with sample data

---

## Connection Settings

The application connects to MySQL using the following defaults:

| Setting | Value |
|---|---|
| Server | `localhost` |
| Port | `3306` |
| Database | `client_schedule` |
| Username | `sqlUser` |
| Password | `Passw0rd!` |

> If your MySQL credentials differ from the defaults, update them in `Data/DBConnection.cs`.

---

## Running the Application

1. Clone or download the repository
2. Open the solution in **Visual Studio 2019+**
3. Restore NuGet packages (MySql.Data v9.6.0)
4. Complete the [Database Setup](#database-setup) steps above
5. Build and run the application

**Default login credentials:**
- **Username:** `test`
- **Password:** `test`

---

## Database Schema

The `client_schedule` database consists of the following tables:

- **`country`** – Stores country reference data
- **`city`** – Cities, linked to a country
- **`address`** – Street addresses, linked to a city
- **`customer`** – Client records, linked to an address
- **`user`** – Application user accounts
- **`appointment`** – Scheduled appointments, linked to a customer and user

The sample data includes 2 countries, 3 cities, 3 customers, and 5 upcoming appointments.
