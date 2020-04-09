# FireDoor
## Description
Firedoor is a .NET 4.8 application that allows users to measure the temprature of their CPU's cores without having to manually monitor them.
It was created from a need to measure core temps from overclocked CPUs to ensure that long-term heat would not damage the chip.  The user
starts by specifying which resource-intensive applicaiton they plan to test.  The user will then have an oppurtunity to define the max
temprature they are willing their CPU cores to reach.  The application specified will then start, and FireDoor will monitor the tempratures
in the background.  If a core goes past the maximum temprature specified, FireDoor will exit the test app to help prevent long term heat
damage from being done to the CPU.  

## Dependencies
- OpenHardwareMonitor: A DLL of OpenHardwareMonitor has been included in this app.  It includes libaries that allow the developer to 
measure CPU temps.  Unfortunately, no Nuget package for this libaray was availble, which is why the DLL is in source control.  The library
also targets .NET 2.0, which is why FireDoor is written in .NET 4.8 instead of .NET Core.

## Please Note!!!
This applicaiton is in the final phases of development.  Once finished, proper documentation will be provided in this readme on how it can 
be used.  
