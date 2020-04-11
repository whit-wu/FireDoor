# FireDoor
## Description
Firedoor is a .NET 4.8 application that allows users to measure the temprature of their CPU's cores without having to manually monitor them.
It was created from a need to measure core temps from overclocked CPUs to ensure that long-term heat would not damage the chip.  The user
starts by specifying which resource-intensive applicaiton they plan to test.  The user will then have an oppurtunity to define the max
temprature they are willing their CPU cores to reach.  The application specified will then start, and FireDoor will monitor the tempratures
in the background.  If a core goes past the maximum temprature specified, FireDoor will exit the test app to help prevent long term heat
damage from being done to the CPU.  It should be noted that FireDoor is NOT meant to overclock CPUs; it acts as supplemental software that allows users to safely test their overclocked settings.  

##

## Dependencies
- OpenHardwareMonitor: A DLL of OpenHardwareMonitor has been included in this app.  It includes libaries that allow the developer to 
measure CPU temps.  Unfortunately, no Nuget package for this libaray was availble, which is why the DLL is in source control.  The library
also targets .NET 2.0, which is why FireDoor is written in .NET 4.8 instead of .NET Core.

## FAQ
<strong>Why are there no unit tests?</strong>

Unit tests to measure core temps were written at the start of the project, however the tests did not have sufficent permissions to access the temprature of each core.  Initial research did not yield any fixes to this issues, so tests were ommitted.  

<strong>What is the app.manifest file for?</strong>

The app.manifest file allows Visual Studio to run the code with admin privileges.  Without it, the app is unable to read the temprature of each core.

<strong>FireDoor sees more cores on my CPU thank I am expecting.  What's the deal?</strong>

FireDoor uses a DLL from OpenHardwareMonitor to call libaries that will read the temprature of each logical core (the number of physcial cores on your CPU times the amount of threads each core runs).  For example, let's say my CPU has two physical cores, and each core can run two threads.  OpenHardwareMonitor will seem them as four cores when reading CPU core tempratures on my PC.  

## Please Note!!!
This applicaiton is in the final phases of development.  Once finished, proper documentation will be provided in this readme on how it can 
be used. 

