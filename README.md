Backend project for Gym Management System built with **ASP.NET Core 8**, **Entity Framework Core**, and **ASP.NET Identity**.
--
Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- SQL Server (LocalDB or SQL Server Express recommended)
- Git
Project Structure
GymAngel/
├── GymAngel.Api/ # Web API project (entry point)
├── GymAngel.Data/ # Data layer (DbContext, EF migrations)
├── GymAngel.Domain/ # Domain entities (User, Role, etc.)
├── GymAngel.Business/ # Business logic / Services
├── GymAngel.sln
└── .gitignore

Setup
1. **Clone repo**
   git clone https://github.com/turkeydei/gym-angel.git
2. **Install dependencies :**  
-dotnet restore
3. **Create migration (if first time):** 
-dotnet ef migrations add InitialCreate -p GymAngel.Data -s GymAngel.Api
4. **Update database :**
-dotnet ef database update -p GymAngel.Data -s GymAngel.Api
5. Run the API :
run tại project GymAngel.API
-dotnet run

