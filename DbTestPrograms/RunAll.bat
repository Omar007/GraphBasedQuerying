@echo off

SET programParameters=--output ".\\EntityGraphTestResults\\" --affinity 2 --minimumruntime 1000 --repeat 10 --blankruns 2

.\SqlServer\bin\Release\SqlServer.exe %programParameters%
.\Postgresql\bin\Release\Postgresql.exe %programParameters%
.\MySql\bin\Release\MySql.exe %programParameters%
