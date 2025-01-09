![image](https://github.com/user-attachments/assets/9ecaa340-1a87-4612-93cc-914205f53f59)

Database-Driven ASP.NET Core MVC Web Application
This project is a database-driven ASP.NET Core MVC web application developed for the PROG2230: Programming Microsoft Web Technologies course at Conestoga College. The application demonstrates various advanced web development features, focusing on structuring and managing complex data models, implementing business logic, and creating reusable and maintainable components.

Key Features:
Customer Management:
•	View Customers grouped alphabetically (e.g., A–E).
•	Add, edit, and soft delete Customers with validation requirements.
•	Undo soft delete actions within a limited time window using a dismissible message or fade-out effect.

Invoice and Line Item Management:
•	View Customer invoices, including line items and totals.
•	Add new invoices or line items dynamically, with updated totals.

Rich Data Model with Relationships:
•	Implementation of 1-to-1 and 1-to-many relationships using Entity Framework Core.
•	Navigation properties to handle entity relationships (e.g., Customers ↔ Invoices ↔ Line Items).

Core Development Practices:
•	Business logic encapsulated in a scoped service.
•	Use of LINQ for querying and manipulating data.
•	Support for data validation constraints and reusable components using Razor views and partial views.
•	Paging solution for grouping Customers alphabetically.
Soft Delete Pattern:
•	Implementation of "soft delete" functionality to mark records as deleted while preserving their data in the database.
•	Undo actions for deleted records.

Unit Testing:
•	Automated unit testing of business logic using xUnit with at least three test cases.
•	Tools and Technologies:
•	ASP.NET Core MVC for application structure and web framework.
•	Entity Framework Core for database interaction.
•	Razor Views and Partial Views for UI components.
•	xUnit for unit testing.
•	LINQ for querying and manipulating data.

Project Highlights:
This project serves as a practical exercise in applying best practices for web application development. It demonstrates:
•	Effective use of hypermedia design to link related entities.
•	Implementation of validation requirements for forms and data input.
•	Reusable components for maintaining a clean and consistent user interface.
•	Requirements and Guidelines:
•	Custom validation for input fields such as phone numbers, email addresses, and postal/ZIP codes.
•	A class library for domain entities and reusable components.
•	Marking schema compliant with project guidelines to ensure academic integrity.

Feel free to explore the project code to learn more about the design, architecture, and implementation of a full-stack database-driven web application.
