# TraceEfChanges
This project show, how to trace the changes made in diferent entities created with EF core.

To make it works, you have to create an SQL Server data base and write the connection string in the method OnConfiguring(DbContextOptionsBuilder optionsBuilder) of the class TraceContext, under the namespace TraceEFChanges.DataAccess 
