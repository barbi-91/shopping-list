# shopping-list
Make a Grocery List 

Barbara Erdec Golović

This GUI application consists of WPF application that create simple and organized shopping list and enable to copy, save or print or edit currently created list relational database in the 
form of an mdf file that can also be used within a SQL server.

INTRODUCTION  
This article describes how to build a C# application that create simple shopping list of grocery, using a database file in form of mdf. and ldf. file. App can copy list using copy 
to cliboard  lets you easily share those lists with your contacts, save list to .txt, .doc... file. Print list to PDF/ printer. It is also possible to delete the entire current list 
or the ability to edit existing content. No pen or paper required. In addition to allowing users to create a fun, and even keep an updated home pantry inventory for reference. Simple, 
and incredibly easy to use.

Application come in Windows - .zip file build which includes .exe file, .mdf and .ldf dependencies which need to be extracted in directory and .exe can be run after that and be able 
to use database.

BACKGROUND
There is a need for such an application when you keep organized your list — no pen or paper required. To print, option is also included. So MDF and LDF files are created automatically 
when a new database is created in the program. The .mdf extension file contains all the startup data for the database to run and monitor all databases on the SQL server. It also points 
to other files in the database. This file is a key data storage file that is very important when receiving and monitoring data content on the server. LDF saves database data and keeps 
track of all actions and changes to information on the server. The changes that .ldf files often record include; delete files, insert, warn, update and update. Usually .LDF is an 
associated file in .MDF when creating a new database or when creating a backup file. MDF file is the primary file in SQL server database. The LDF is a supporting file. The latter stores 
the information related to transaction logs. MDF contains database record data. LDF, on the other hand records information related to changes made in the server as well as all the 
actions performed. Source code might be useful to developers who have interest in using database.

DEVELOPMENT TOOLS
These were the tools used for development of version 1.0.0.0 of shopping-list (in case of later development versions may change):
Windows - Visual Studio 2019 IDE,. Compiler was MSVC. IDE was configured to use Windows edition of Votive2019.vsix installed from 
downloaded wixWindows installer.

HOW IT WORKS
A user insert item, check if is unique, and amount, using validation numeric number, into shopping list and use click-event: add item:

-loaded MDF and LDF files in SQL Server.
-created a table in database and primary key: id.
-created string sql-connection opening.
-binding data from database to the table using sqlcommand: Select.
-adding data to database by opening connection, and SqlCommand: Insert.
-compare whether the item are unique, and check amount number using validation.
-update/edit data in database by opening connection, and sqlcommand: Update.
-delete data from database by opening connection, and sqlcommand: Delete.
-use SqlDataAdapter in conjunction with SqlConnection and SqlCommand to increase performance when connecting to a SQL Server database.

POINTS OF INTEREST
MDF- primary file in SQL server database. LDF- supporting file. The latter stores the information related to transaction logs. 
MDF contains database record data. LDF, on the other hand records information related to changes made in the server as well as all the 
actions performed.
OS versions on which this software was tested are Windows 10, setup toolset Votive2019.vsix installed from downloaded wixWindows installer. 

HISTORY
-	1.0.0.0 - 2021-16-11 - Initial version

LICENSE
This article, along with any associated source code and files, is licensed under GNU GENERAL PUBLIC LICENSE Version 3, 29 June 2007.

Screenshot (viewer application):
![screenshot](./screenshot.png?raw=true)

 
