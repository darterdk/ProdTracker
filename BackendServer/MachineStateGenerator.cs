using BackendServer.Models;
using Microsoft.AspNetCore.SignalR;

namespace BackendServer;

public class MachineStateGenerator : BackgroundService
{
    private readonly IHubContext<MachineStateHub> _hubContext;

    private readonly Dictionary<Guid, string> _machineOrders = new()
    {
        { new Guid("c7d93c96-002a-49a2-9008-db8398ba646b"), "InjectionMoulding" },
        { new Guid("dd6de547-c60c-4f78-805c-365a69312366"), "InjectionMoulding" },
        { new Guid("c39e1298-0d3d-4a0f-a01b-b75bb45c8f3a"), "InjectionMoulding" },
        { new Guid("3564497c-c25b-49da-9107-79a7d3d0dd49"), "Sorting" },
        { new Guid("f58304b3-f483-424b-a7b1-c3a820b08c68"), "Sorting" },
        { new Guid("a4e633db-dadc-4b1c-b20b-933c8cf3f601"), "Packing" },
        { new Guid("5257168d-57bb-4598-9047-f03a52544667"), "Printing" },
        { new Guid("e9d8c6c8-f93f-4f2a-bfbc-2a842d31d55d"), "Printing" },
    };

    public MachineStateGenerator(IHubContext<MachineStateHub> hubContext)
    {
        _hubContext = hubContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(1.5));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await GenerateMachineStatesAsync(stoppingToken);
        }
    }

    private async Task GenerateMachineStatesAsync(CancellationToken cancellationToken)
    {
        var order = GetRandomMachineOrder();
        var machineStateEvent = new MachineStateEvent
        {
            OrderGuid = order,
            ProductionState = GetRandomState(),
            MachineName = _machineOrders[order]
        };

        await _hubContext.Clients.All
            .SendAsync("ReceiveMachineStateEvent", machineStateEvent, cancellationToken);
    }

    private Guid GetRandomMachineOrder()
    {
        List<Guid> keyList = new List<Guid>(_machineOrders.Keys);
        Random random = new Random();
        Guid randomGuid = keyList[random.Next(keyList.Count)];

        return randomGuid;
    }
    
    private MachineState GetRandomState()
    {
        var values = Enum.GetValues(typeof(MachineState));
        Random random = new Random();
        MachineState randomState = (MachineState)values.GetValue(random.Next(values.Length))!;

        return randomState;
    }
}

