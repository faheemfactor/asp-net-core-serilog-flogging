# asp-net-core-serilog-flogging
This is implementation of serilog.net logging into dotnet core console application and asp.net core 2.0 MVC web application.  For more information please visit https://serilog.net/ , https://www.asp.net/core/overview/aspnet-vnext , https://github.com/dahlsailrunner
Please modify/clean this code as per your need, code helps you to know about serilog and potential to use it across different levels of application.
Credit to https://github.com/dahlsailrunner for providing insight into the serilog.


## How to use it
Use Flogging.Console application as startup for first time. As this will display you in case if there is issue in your database connection or any relevant issue in basic configuration. It should create the four tables in relevant database
you can run this code if the console applciation does not show any error.

SELECT * FROM [dbo].[PerfLogs]
SELECT * FROM [dbo].[UsageLogs]
SELECT * FROM [dbo].[ErrorLogs]
SELECT * FROM [dbo].[DiagnosticLogs]

Then change the Flogging.Web to start up project and run the web application.

## For File Log
There is a code in Flogging.Core commented. Enabling that will create files in application direction folder log.

## configuring MSSQL database connection
Update the database relevant configuration in Project Flogging.Core -> Flogger.cs , go to line 24

var connectionObject =
              new
              {
                  server = "",
                  database = "",
                  username = "",
                  password = ""
              };
