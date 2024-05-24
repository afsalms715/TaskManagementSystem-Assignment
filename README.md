instructions for set up the application

1 Restore NuGet Packages:

  Open the solution in Visual Studio.
  Restore NuGet packages if they are not automatically restored (Build > Restore NuGet Packages).
  Configure the Database Connection:

2 Edit the appsettings.json file to configure the connection string for their local database.
  {
      "ConnectionStrings": {
          "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=YourDatabaseName;Trusted_Connection=True;MultipleActiveResultSets=true"
      }
  }

3 Apply the Migrations:

  Open the Package Manager Console.
  Run the Update-Database command to apply the migration and create the database schema.

  Update-Database

  <img width="956" alt="image" src="https://github.com/afsalms715/TaskManagementSystem-Assignment/assets/81500617/013d880c-523b-4773-afb2-0f69b673fc2b">
