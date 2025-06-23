# EventEase – POE Part 3 Submission (CLDV6211)

## 👤 Student Information
- **Name:** Morné Viljoen  
- **Student Number:** [Your Student Number]  
- **Module:** Cloud Development (CLDV6211)  
- **Lecturer:** [Your Lecturer’s Name]  

---

## 📺 Azure Demonstration Video

> ▶️ **Link to video demonstration:**  
> [https://youtu.be/YOUR-UNLISTED-VIDEO-LINK](https://youtu.be/YOUR-UNLISTED-VIDEO-LINK)

This video includes:
- All Azure resources (App Service, SQL DB, Blob Storage)
- Live Azure app with:
  - CRUD for Events, Venues, and Bookings
  - At least 5 bookings
  - Event image uploads
  - Venue availability filtering logic
  - Full search & advanced filtering

---

## 🌐 Azure Resources Used

| Resource        | Purpose                                  |
|-----------------|------------------------------------------|
| **App Service** | Hosts the deployed ASP.NET Core MVC app |
| **SQL Database**| Stores all application data (EF Core)    |
| **Blob Storage**| Stores event and venue images securely   |

---

## ✅ Features Implemented in Part 3

### 🔍 Advanced Filtering (20/20)
- Filter bookings by **Event Type**
- Filter bookings by **Date Range**
- Filter bookings by **Venue Availability**

### 🌐 Azure Deployment
- Final web app deployed to:  
  `https://STXXX.azurewebsites.net` (Replace with your real URL)
- Updated SQL DB with:
  - New field: `Availability` in Venue Table
  - Screenshot of Query Editor included
- Blob Storage used for image uploads

---

## 📦 CRUD Functionality Summary

- ✅ **Venues**: Create, Edit (change availability), Delete  
- ✅ **Events**: Create with image upload, Edit, Delete  
- ✅ **Bookings**: Create, Edit, Delete  
- ❌ Unavailable venues are hidden from booking form  
- ✅ Filtering & search implemented in the Booking Index view  

---

## 💭 Reflective Summary

This project taught me a lot about **building cloud-based applications**. I encountered many errors along the way — from incorrect firewall settings, database update issues, Blob Storage misconfigurations, and controller logic bugs — but I overcame them step-by-step.  
Through fixing each issue, I learned how to:
- Use Entity Framework Core with SQL Azure  
- Deploy a .NET app to Azure App Services  
- Work with Azure Blob Storage for media handling  
- Write reusable, maintainable controller and view logic

I now have a much stronger understanding of how to **design, develop, and deploy cloud-based MVC applications**. Although I know the system still isn’t perfect, I’m proud of how far it has come.

---

## 📘 Referencing

- Ciampa, M. (2022). *CompTIA Security+ Guide to Network Security Fundamentals*. Cengage Learning.  
- Microsoft Docs. (n.d.). Azure App Service, SQL Database, Blob Storage documentation.  
  [https://learn.microsoft.com/azure](https://learn.microsoft.com/azure)  
- Stack Overflow, GitHub Discussions, and documentation for error resolutions.

---

## 📂 File Structure Submitted
- ✔️ Updated source code (Events, Venues, Bookings)
- ✔️ Azure screenshots (Query editor, filtering)
- ✔️ Reflective report
- ✔️ Video link (above)
