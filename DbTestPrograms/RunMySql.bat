@echo off

SET programParameters=--output ".\\EntityGraphTestResults\\" --affinity 2 --minimumruntime 500 --repeat 10 --blankruns 5

.\MySql\bin\Release\MySql.exe %programParameters%
