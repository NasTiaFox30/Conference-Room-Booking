# Conference Room Booking System

## Technologies:
.NET Framework (ASP.NET Web Forms)
HTML, CSS, ASP.NET Server Controls
MS SQL Server

## Started - Ended
8.11.2025 - 25.11.2025
### Hours
80+ hours

## Application:
The Conference Room Booking System is a centralized web application developed using ASP.NET Web Forms (.NET Framework) designed to optimize the reservation of meeting spaces within an organization. It replaces manual scheduling with an automated solution that ensures transparency and efficiency.

## Features:
Real-time Availability: Users can view a calendar of all conference rooms to check availability instantly.
Advanced Search: Filter rooms by capacity, date, time, and available equipment (e.g., projectors, Wi-Fi).
User & Admin Roles: Distinct interfaces for general employees to book rooms and for administrators to manage room details, confirm & cancel reservations.


## Libraries ( NuGet )
BCrypt.Net-Next NuGet package

# 🚀 How to start app locally?
#### Creating SQL DataBase
1) Create Local DB in folder App_Data
   
   ```Add > New item... > SQL Server DataBase```
2) Click on created DB & in properties window search 'Connection String' (copy)
3) In File Web.config:
   
   Search ```<ConnectionStrings>```
   
   Change parameter:  ```connectionString="``` (paste copied string here)

#### Create Structure
1) Double click on created DataBase
2) Right mouse click ```ConferenceRoomsBookingSystem.mdf > NewQuery```
3) Copy/paste first query file - SQLQuery1.sql into created one
4) Execute All
5) Copy/paste second query file - SQLQuery2.sql into created one
6) Execute Cleanup then rest of query
7) Refresh DB

#### Start Application
Run the application on localhost by pressing F5

---------------------------------------------------------------
_**Creator: Anastasiia Bzova 2025**_
