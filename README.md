The Nexus has been created to allow a user to input information about people in their lives.  The API is how we retrieve this information from the databases where they are stored.  
It also contains the Scripts to generate the database.

Needs to run:

You will need to add an appsettings.json similar to below.  Input your own server name and you can change the name of the database as needed.
********************************
{
  "ConnectionStrings": {
    "DefaultConnection": "server=**Enter server name here**;database=TheNexus;trusted_connection=true;Encrypt = False"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
********************************

Need to run one of the Script files named below to generate the blank tables or create one of your own.  Just replace instances of 'TheNexus' or 'Test' in the files to what your 
databases are called then make sure to change the appsettings.json file accordingly.
TheNexus-Script.sql
Test-Script.sql
