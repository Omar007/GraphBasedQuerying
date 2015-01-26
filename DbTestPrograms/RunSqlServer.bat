@echo off

SET programParameters=--output ".\\EntityGraphTestResults\\" --affinity 2 --minimumruntime 500 --repeat 10 --blankruns 5

.\SqlServer\bin\Release\SqlServer.exe %programParameters%
