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

Or you can use my database to test my project I left the file “appsettings.Development.json” open. But the possibilities of the database are limited! 
Use login: Davidenko password: admin
You may also need to change the API connection settings in the client side of the application. 
For example, you may have different numbers in the local host: 
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "APIUrlSettings": {
    "EmployeerUrl": "https://localhost:7251/Admin",
    "CustomerUrl": "https://localhost:7251/Customer",
    "OrderUrl": "https://localhost:7251/Order",
    "AdminUrl": "https://localhost:7251/Admin",
    "AuthUrl": "https://localhost:7251/Auth/Login-employeers"
  },
  "Jwt": {
    "Key": "MySuperSecretKey1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ!",
    "Issuer": "localhost"
  }
}
```
In this example I have a host with 7251 digits you may have a different host ! 
Look on which host you are running the API! 
Just swap out the numbers on the placeholders for your own.

### 5. Run the Project

In order for the project to fully work you need to manually create a worker with the role “admin” in MongoDB. 
This can be done through the API, which you can run separately and use “POST /Admin/register-new-employeer” endpoint. 
In Admin project endpoints are set without authorization, if you want to enable it just remove comments in this line in “AdminController.cs” file in API:

```C# 

    [ApiController]
    [Route("[controller]")]
    //[Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IEmployeerRepository _employeerRepository;

        public AdminController(IEmployeerRepository employeerRepository)
        {
            _employeerRepository = employeerRepository;
        }
            .........
```
Change: //[Authorize(Roles = "admin")] to [Authorize(Roles = "admin")]

You can then go into the profile for that worker and use the app. 
It is IMPORTANT that the user role is “admin”.

Or you can use my database to test my project I left the file “appsettings.Development.json” open. But the possibilities of the database are limited! 
Use login: Davidenko password: admin

### 6. Important information

The project is in the early stages of development and it is not perfect. Full possible code refactoring and design improvement is planned. So far the project is at the MVP (minimum viable product) stage.

If you have any questions about the project, please contact me: artursdavidenko@gmail.com.
I am always open to communication !  


  


