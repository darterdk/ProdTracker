namespace BackendServer.Models;

public class MachineStateEvent
{
    public Guid OrderGuid { get; set; }
    public MachineState ProductionState  { get; set; }
    public required string MachineName { get; set; }
}

public enum MachineState
{
    Red,
    Yellow,
    Green
}
