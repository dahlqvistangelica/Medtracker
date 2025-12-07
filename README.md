# Medtracker
# 💊 MedTracker - Digital Medication Tracking (Student Project)

### ⚠️ PROJECT STATUS: UNDER ACTIVE DEVELOPMENT (Work In Progress - WIP)

This project is a student assignment currently undergoing conversion from a .NET Console application to a modern, cross-platform application using .NET MAUI. It's a learning process and should not be seen as a complete and functioning program in any way.

---

## 📘 About the Project

**Original Goal:** To create a simple application for tracking medication inventory, calculating remaining days based on dosage, and notifying the user when it's time to refill a prescription.

**Current Goal:** To future-proof and modernize the application by replacing the text-based interface (Console) with a graphical user interface (GUI) using .NET MAUI.

## 🛠️ Technology and Architecture

* **Framework:** .NET 8 / .NET MAUI (Mobile & Desktop)
* **Design Pattern:** Model-View-ViewModel (MVVM)
* **Database/Persistence:** Local file storage using JSON (`IFileStorage`).
* **Dependency Injection (DI):** Used to manage all services (such as data management and file storage).

---

## 🚀 Completed Development (To Date)

The following steps have been implemented to build the MAUI application and separate the application logic:

### 1. Core Logic & Structure
* **Class Library (Core Logic):** All business logic (`Medication.cs`, `DataManager.cs`, `IHandlerRepo`, `IFileStorage`) has been moved to a platform-agnostic Class Library.
* **Robust Persistence:** The system now handles loading data from file at startup and saving data directly to JSON after every change, managed via a Factory Method in `MauiProgram.cs` to avoid `NullReferenceException` errors.
* **MVVM Base:** Fundamental ViewModels (`MainPageViewModel`, `AddMedicationViewModel`, `ShowMedicationsViewModel`) are created and successfully bound to the Views.

### 2. User Interface (Views)
* **Main Page (`MainPage`):** Contains the basic navigation menu and displays critical (soon-to-expire or depleted) medications using a `CollectionView`.
* **Add Medication Page (`AddMedicationPage`):**
    * Uses `DatePicker` and `Picker` for platform-native input.
    * **Conditional Visibility:** The input field for "Medication Scope" dynamically shows/hides based on the selected `MedicationType` (e.g., hidden if the type is Pill/Fluid/Injection).
* **Show Medications Page (`ShowMedicationsPage`):** Implemented with `CollectionView` to list all current medications.

---

## 🛣️ Future Development (Next Steps)

1.  **Implement Core Features:** Build out the logic and UI for **Edit** and **Remove** medications (based on the existing logic in `MedicationHandler.cs`).
2.  **Enhance Display:** Add functionalities for filtering and searching the medication list.
3.  **UI/UX:** Further improve the design and user experience.
