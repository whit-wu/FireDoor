using OpenHardwareMonitor.Hardware;

namespace FireDoor.Services
{
    // The code below was pulled from OpenHardwareMonitor's
    // main project.  It was used by the main project to 
    // communiate with another project, called OpenHardwareMonitorLib,
    // which was the portion of the app that pulled hardware data.
    // Since the OpenHardwareMonitor DLL only includes the contents
    // of OpenHardwareMonitorLib, we need to recreate the UpdateVisitor
    // class in our project so we can pull down hardware data just as 
    // OpenHardwareMonitor did.
    public class UpdateVisitor : IVisitor
    {
        public void VisitComputer(IComputer computer)
        {
            computer.Traverse(this);
        }
        public void VisitHardware(IHardware hardware)
        {
            hardware.Update();
            foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
        }
        public void VisitSensor(ISensor sensor) { }
        public void VisitParameter(IParameter parameter) { }
    }
}

