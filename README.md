# FilteredLogging

## What is it about ? ##

This is a proof of concept on how we could setup dynamic logging where the logging level depends on arbitrary criteria which can evolve dynamically.

Sample is configured to run in debug mode with a SignalR Serilog sink and a default local Redis for notification across nodes (though this sample only features one node).
A way to have a local Redis :

`docker run -d -p 6379:6379 --name redis redis:latest`

If you want to emulate some multitenancy on Windows, you can edit your `hosts` file and put
a line with `*.localhost 127.0.0.1`. You will then be able to call the application with  

http://*tenantName*.localhost:3000

