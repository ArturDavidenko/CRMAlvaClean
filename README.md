# Customer relationship management prototype for "AlvaClean" company.

## 📝 Description
This project is a prototype CRM system tailored for "AlvaClean" cleaning company. It provides a centralized platform to manage:

- Orders: Track cleaning orders, their status, and details.
- Customers: Store and manage information about client companies.
- Employees: Keep records of employees, their roles, and assignments.

The system is designed to streamline operations, improve organization, and enhance productivity for cleaning businesses. While this is a prototype, it lays the foundation for future improvements and additional features.

## 🛠️ Technology Stack

| **Category**       | **Technologies**                              | **Details**                                   |
|---------------------|-----------------------------------------------|-----------------------------------------------|
| **Framework**       | .NET 8                                        | Cross-platform, high-performance framework    |
| **Web Framework**   | ASP.NET                                       | For building web applications and APIs        |
| **API**             | REST API                                      | Standard for communication between services   |
| **Authentication**  | JWT (JSON Web Tokens)                         | Secure token-based authentication             |
| **Database**        | MongoDB                                       | NoSQL database for flexible data storage      |
| **ORM**             | MongoDB Driver                                | For database interactions                     |
| **Testing**         | xUnit / NUnit (soon)                          | Unit and integration testing                  |
| **Logging**         | Serilog (soon)                                | Structured logging                            |
| **Deployment**      | Docker                                        | Containerization                              |


## 🛠️ Installation

### 1. Clone the Repository
   
First, clone the repository to your local machine using Git:
``` git clone https://github.com/ArturDavidenko/CRMAlvaClean.git ```

### 2. Launch Visual Studio.

Open the solution file (AlvaCleanCRM.sln) from the cloned repository.
Or open solution as folder.

### 3. Restore Dependencies
Restore the required NuGet packages:

- Right-click on the solution in Solution Explorer and select Restore NuGet Packages.

- Alternatively, run the following command in the Package Manager Console:
``` dotnet restore ```

### 4. Set Up the Database

- Connect MongoDB to solution. Change connection string in `appsettings.Development.json` in AlvaCleanAPI folder:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "DataContext": {
    "MongoUrl": "Write your own connection string",
    "MongoDbName": "Write your own db name"   
  },
  "Jwt": {
    "Key": "MySuperSecretKey1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ!",
    "Issuer": "localhost"
  }

} 
```
Jwt space can be not replaced.

### 5. Run the Project






  


