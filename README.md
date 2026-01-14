# ProdTracker and OrderView
 
ProdTracker is a backend service that correlates order data with machine production states on the factory floor. This service uses an event-based system to communicate changes in production machine states to a subscribed frontend application, OrderView via SignalR. The sample code here generates new machine states every 1.5 seconds.

The application consists of two solutions:
* ProdTracker (backend): ASP.NET Core Web Api with integrated SignalR for communication.
* OrderView (frontend): React + TypeScript single page application with a SignalR hub for receiving push updates from the backend.
 
To run the applications:
1. Open ProdTracker in your IDE of choice and start the BackendServer: http.
2. Change working directory to: \ProdTracker\frontend\machinestate-ui and execute the command npm run dev.
3. Go to http://localhost:5173 to see OrderView frontend. 

OrderView displays updated production machine states for associated orders.
