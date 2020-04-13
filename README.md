# FireDoor
## Description
Firedoor is a .NET 4.8 application that allows users to measure the temperature of their CPU's cores without having to manually monitor them.
It was created from a need to ensure that long-term heat would not damage the chip.  The user starts by specifying which resource-intensive application they plan to test.  The user will then have an opportunity to define the max temperature they are willing their CPU cores to reach.  The application specified will then start, and FireDoor will monitor the temperatures in the background.  If a core goes past the maximum temperature specified, FireDoor will exit the test app to help prevent long term heat damage from being done to the CPU.  It should be noted that FireDoor is NOT meant to overclock CPUs; it acts as supplemental software that allows users to safely test their overclocked settings.  
 
## Dependencies
- OpenHardwareMonitor: A DLL of OpenHardwareMonitor has been included in this app.  It includes libraries that allow the developer to 
measure CPU temps.  Unfortunately, no Nuget package for this library was available, which is why the DLL is in source control.  The library
also targets .NET 2.0, which is why FireDoor is written in .NET 4.8 instead of .NET Core.
 
## FAQ
<strong>What is the app.manifest file for?</strong>
 
The app.manifest file allows Visual Studio to run the code with admin privileges.  Without it, the app is unable to read the temperature of each core.
 
<strong>FireDoor sees more cores on my CPU than I am expecting.  What's the deal?</strong>
 
FireDoor uses a DLL from OpenHardwareMonitor to call libraries that will read the temperature of each logical core (the number of physical cores on your CPU times the amount of threads each core runs).  For example, let's say my CPU has two physical cores, and each core can run two threads.  OpenHardwareMonitor will seem them as four cores when reading CPU core temperatures on my PC.  
 
## Please Note!!!
This application is in the final phases of development.  Once finished, proper documentation will be provided in this readme on how it can 
be used. 
