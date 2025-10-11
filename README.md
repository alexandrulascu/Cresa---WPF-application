#  Cresa - WPF Application (MVVM)

A desktop management system for a kindergarten (**“Creșa”**), developed in **C# / WPF** and structured using the **MVVM (Model-View-ViewModel)** design pattern.  
This project was created as part of a university assignment and demonstrates clean architecture, UI separation, and structured module interaction.

---

##  Overview

The application provides three main user modules:
-  **Admin** – manages parents, educators, children, groups, and payments  
-  **Educator** – manages groups and notifications for parents  
-  **Parent** – receives notifications and views child information  

The goal is to create a modular WPF system that follows **MVVM principles**, maintaining a clear separation between:
- **Views** → XAML UI definitions  
- **ViewModels** → Presentation logic and commands  
- **Models** → Data entities and database interaction  

---

##  Tech Stack

- **C# / .NET Framework 4.7.2**
- **WPF (Windows Presentation Foundation)**
- **MVVM architecture**
- **SQL Server** (for local data storage)
- **Visual Studio 2022**

---

##  Project Structure

```markdown

Cresa
│
├── Commands # RelayCommand and base command classes
├── Models # Database and data models
├── Resources # Image and style resources
├── Services # Database connection and logic services
├── ViewModels # MVVM ViewModel classes
└── Views # UI (XAML) files for each module
├── Admin
├── Educator
├── Parinte
├── LoginWindow.xaml
└── MainWindow.xaml
```
---

##  How to Run

-  Clone this repository:
```bash
git clone https://github.com/alexandrulascu/Cresa---WPF-application.git
```

-  Open the solution in Visual Studio 2022
-  Restore NuGet packages (if required)
-  Run the project — the LoginWindow will appear first
-  Navigate through the modules (Admin, Educator, Parent)
