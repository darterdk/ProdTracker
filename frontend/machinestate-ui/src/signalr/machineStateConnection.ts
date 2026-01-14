import * as signalR from "@microsoft/signalr";
import type {MachineProductionState} from "../models/MachineProductionState";

export function createMachineStateConnection(
    onEvent: (event: MachineProductionState) => void
) {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:5239/machineStateHub")
        .withAutomaticReconnect()
        .build();

    connection.on("ReceiveMachineStateEvent", onEvent);

    return connection;
}
