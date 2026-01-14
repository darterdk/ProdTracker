using BackendServer.Models;
using Microsoft.AspNetCore.SignalR;

namespace BackendServer;

public class MachineStateHub : Hub
{
    public async Task SendMachineState(MachineStateEvent stateModel)
    {
        await Clients.All.SendAsync("ReceiveMachineStateEvent", stateModel);
    }
    
}