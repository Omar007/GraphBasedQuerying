@echo off

SET programParameters=--output ".\\EntityGraphTestResults_Merijn\\" --affinity 2 --minimumruntime 250 --repeat 10 --blankruns 5

.\Merijn\bin\Release\Merijn.exe %programParameters%
