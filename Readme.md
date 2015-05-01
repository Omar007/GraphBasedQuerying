# Graph-Based Querying

Code associated with the thesis written by Omar Pakker at the University of Amsterdam.  
Based on work of Merijn de Jonge (http://www.codeproject.com/Articles/247254/Improving-Entity-Framework-Query-Performance-Using).  


# Directory structure
The directory structure is as follows:  

### PerformanceFramework
This project forms the base for the performance testing we performed in the thesis. It handles running, timing and aggregating possible failures for implemented performance tests.  
It implements code from http://www.codeproject.com/Articles/42492/Using-LINQ-to-Calculate-Basic-Statistics to quickly calculate statistics.  
**BE AWARE: While this is implemented as a Portable Class Library, it has a dependency on kernel32.dll if you want to use the ExecutionStopwatch!** Until we can implement this without using kernel32.dll, this is something to watch out for.  

### DbTestBase
Contains the base projects for testing different models. Depends on the PerformanceFramework projects. It contains 3 sub-projects:  
- DbTest.Core  
  Contains the base code that manages tests and output.  
- DbTest.Merijn  
  Contains the models originally used by Merijn de Jonge.  
- DbTest.ModelDefinitions  
  Contains the models used in the thesis.  

The last two projects implement the models, the generators to create the databases/schemas and the tests.  

### DbTestPrograms
Contains the programs that run the db tests on specific databases. The projects in here depend on the DbTestBase projects.  
Each sub-project configures the connection for the given database.  

The projects OracleDb and IBMDB2 have a skeleton set up but have not been completed.  
They are currently in a non-working state due to either the library for EF6 being in development, requiring payment or lacking functionality such as code-first models.  

The projects MySql, Postgresql and SqlServer and in working order and can be used to run tests.  
The project 'Merijn' is also available to run the models originally used by Merijn on this testing framework and EF6.  

Batch files are provided to easily execute the programs with some standard settings.  

### Graph-Based Querying
This directory contains the main project; the implementation of Graph-Based Querying on top of the Entity Framework.  
It contains two projects with accompanying Unit Tests, implemented with xUnit.  
- EntityGraph (Portable Class Library)  
  The core project for Graph-Based Querying. It implements the base functionality to represent data as a graph and the interfaces to create Graph-Based Querying implementations.
- EntityGraph4EF6  
  This project implements Graph-Based Querying on the Entity Framework. It extracts mapping information from the EF model and uses this information to create new queries based on graph shapes defined by the programmer.  
  The current implementation builds a custom SQL tree to generate the queries but a WIP branch is present where we attempt to utilize DbExpressions instead.  

### Extras
This folder contains a basic application of the idea of using Graphs to query for data.  
Graph4EF implements Graph shapes only by wrapping around Entity Framework function calls; it does not actually build or execute any queries on its own.  

The Graph4EF implementation uses functionality/code from https://github.com/loresoft/EntityFramework.Extended  

##### LoreSoft License
Copyright (c) 2012, LoreSoft All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
- Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
- Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
- Neither the name of LoreSoft nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.  
