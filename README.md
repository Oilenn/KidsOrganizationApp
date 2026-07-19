# KidsOrganizationApp

**Desktop application under development** for automating and managing the activities of a non-profit organization supporting families with children with disabilities.

The project is being developed for a real non-profit organization and is intended to replace paper-based records and Excel spreadsheets. Its main goal is to provide centralized management of families, documents, and organizational events.

---

## Technologies

* **C#** – primary programming language
* **WPF** – graphical user interface framework
* **Entity Framework Core** – database access
* **SQLite** – local database

---

## Architecture

* **DDD (Domain-Driven Design)** – business logic is built around the domain model
* **MVVM (Model-View-ViewModel)** – clear separation of the user interface, business logic, and data

---

## Features

### Child and Parent Management

* Store essential information about children and their parents or legal guardians
* Associate a child with a parent or legal guardian
* Associate one parent with multiple children
* Store date of birth, contact phone number, and residential address
* Track organization membership status

### Event Management

* Create organizational events
* Specify the event name and date
* View a list of upcoming and completed events
* Edit and delete events
* Link events with their related documents

### Document Management

* Add documents related to families and events
* Store the file path for each document
* Update the document category and file location
* View documents associated with a selected entity

Supported document categories:

* Passport
* SNILS
* Medical Diagnosis
* Letter
* Order
