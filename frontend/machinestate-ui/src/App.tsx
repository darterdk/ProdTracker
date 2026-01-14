import { useEffect, useState } from "react";
import { HubConnection } from "@microsoft/signalr";
import type {MachineProductionState} from "./models/MachineProductionState";
import { createMachineStateConnection } from "./signalr/machineStateConnection";

function getStateStyle(state: string): React.CSSProperties {
    switch (state) {
        case "Green":
            return { backgroundColor: "#d1fae5", color: "#065f46" }; 
        case "Yellow":
            return { backgroundColor: "#fef3c7", color: "#92400e" }; 
        case "Red":
            return { backgroundColor: "#fee2e2", color: "#991b1b" }; 
        default:
            return {};
    }
}

const thStyle: React.CSSProperties = {
    borderBottom: "2px solid #ccc",
    textAlign: "left",
    padding: "8px"
};

const tdStyle: React.CSSProperties = {
    borderBottom: "1px solid #eee",
    padding: "8px"
};


function App() {
    const [machineStates, setMachineStates] =
        useState<Record<string, MachineProductionState>>({});
    const [, setConnection] = useState<HubConnection | null>(null);

    useEffect(() => {
        const conn = createMachineStateConnection(event => {
            console.log("Received machine event:", event);

            setMachineStates(prev => ({
                ...prev,
                [event.orderGuid]: event
            }));

        });

        conn
            .start()
            .then(() => console.log("SignalR connected"))
            .catch(err => console.error("SignalR connection error:", err));

        setConnection(conn);

        return () => {
            conn.stop();
        };
    }, []);

    return (
        <div style={{ padding: "1rem" }}>
            <h1>OrderView</h1>
            <table style={{ borderCollapse: "collapse", width: "100%" }}>
                <thead>
                <tr>
                    <th style={thStyle}>Order</th>
                    <th style={thStyle}>Machine</th>
                    <th style={thStyle}>State</th>
                </tr>
                </thead>
                <tbody>
                {Object.values(machineStates).map(state => (
                    <tr key={state.orderGuid}>
                        <td style={tdStyle}>{state.orderGuid}</td>
                        <td style={tdStyle}>{state.machineName}</td>
                        <td style={{ ...tdStyle, ...getStateStyle(state.productionState) }}>
                            {state.productionState}
                        </td>
                    </tr>
                ))}
                </tbody>
            </table>

        </div>
    );
}

export default App;
