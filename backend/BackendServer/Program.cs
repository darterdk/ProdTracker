using BackendServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddHostedService<MachineStateGenerator>();

var app = builder.Build();

app.UseRouting();
app.MapHub<MachineStateHub>("/machineStateHub");

app.Run();
