<div align="center">
    <h1> Ragnarok </h1>
</div>

# Ragnarok.AgentApi

## Installation
Install via [Nuget](https://www.nuget.org/packages/Ragnarok.AgentApi/)

# Usage
## Ngrok Process
The `ngrok` client must be active in the background in order to manage tunnels via the Api. As such, methods are provided for launching and closing the `ngrok` process.  
<details>  
  
### Initialize
If an `ngrok` process is already active, the active process is associated to the `RagnarokClient`. However, if there is no active `ngrok` process, one will be launched and associated with the `RagnarokClient`. A process launched by the client is considered to be `managed` by the application. 
```csharp
var client = new RagnarokClient();
await client.InitializeAsync(); //Initialize associates the RagnarokClient with an ngrok client process
```

### StopNgrokProcess
Kills the associated `ngrok` process, **if** that process is `managed` by the appliation. That is, if the process was launched by the application.
```csharp
await client.StopNgrokProcess();
```
### KillNgrokProcess
Kills the associated `ngrok` process, regardless of if that process is `managed` by the application. This means if `ngrok` was launched (externally) before the `RagnarokClient` was initialized, the process **will** be terminated upon executing this method.
```csharp
await client.KillNgrokProcess();
```
</details>

## Api
All endpoints for the  [ngrok Agent API](https://ngrok.com/docs#client-api) are exposed as methods under the [`RagnarokClient`](https://github.com/0phois/Ragnarok/blob/master/Ragnarok.AgentApi/Client/RagnarokClient.Requests.cs).  
> Note: The `RagnarokClient` must be *initialized* before accessing these methods.

<details>
  <summary>Tunnel Methods</summary>  
  
### Tunnels
#### [Start tunnel](https://ngrok.com/docs#start-tunnel)
```csharp
var tunnelDetail = await client.StartTunnelAsync(definition);
```
#### [Get tunnel details](https://ngrok.com/docs#tunnel-detail)
```csharp
var detail = await client.GetTunnelDetailAsync(tunnelDetail.Name);
```
#### [List tunnels](https://ngrok.com/docs#list-tunnels)
```csharp
var tunnels = await client.ListTunnelsAsync();
```
#### [Stop tunnel](https://ngrok.com/docs#stop-tunnel)
```csharp
await client.StopTunnelAsync(detail.Name);
```
</details>

<details>
  <summary>Request Methods</summary>  
  
### Captured Requests
#### [List captured requests](https://ngrok.com/docs#list-requests)
```csharp
var requests = await Ragnarok.ListCapturedRequestsAsync(options => options.Limit = 10);
```
#### [Captured request detail](https://ngrok.com/docs#request-detail)
```csharp
var request = await Ragnarok.GetCapturedRequestDetailAsync(requestId);
```
#### [Replay captured request](https://ngrok.com/docs#replay-request)
```csharp
await Ragnarok.ReplayCapturedRequestAsync(requestId);
```
#### [Delete all requests](https://ngrok.com/docs#delete-requests)
```csharp
await Ragnarok.DeleteCapturedRequestsAsync();
```
</details>

## Extensions
A few [extension](https://github.com/0phois/Ragnarok/blob/master/Ragnarok.AgentApi/Helpers/Extensions/RagnorokClientExtensions.cs) methods are also provided for user convenience 
<details>
  <summary>Register AuthToken</summary>  
  
### [RegisterAuthToken](https://ngrok.com/docs#getting-started-authtoken)
For some of the options provided by ngrok, for example: subdomains, an authtoken is required. To obtain said token, you need to sign up at [ngrok.com](https://ngrok.com/) and check your [dashboard](https://dashboard.ngrok.com/get-started/your-authtoken). Once the authtoken is stored in the ngrok cofigruation yaml, it will be used for all created tunnels.

The `RegisterAuthToken` method adds (or modifies) the authtoken property in your ngrok configuration file: 
```csharp
//should be run before tunnels are started,
//since setting the token will not affect running tunnels.
var registered = await Ragnarok.RegisterAuthTokenAsync(token);
```
</details>  

<details>
  <summary>Connect</summary>  
  
### Connect
The `Connect` method encapsulates calling `InitializeAsync()` and `StartTunnelAsync()`.
```csharp
// defaults to opening an HTTP tunnel to port 8080
var tunnelDetail = await Ragnarok.ConnectAsync(); // https://92832de0.ngrok.io -> http://localhost:8080

// provide an authtoken to be registered before the tunnel is started
var tunnelDetail = await Ragnarok.ConnectAsync(token);

// provide a port number to connect to the specified port
var tunnelDetail = await Ragnarok.ConnectAsync(5555); https://92832de0.ngrok.io -> http://localhost:5555

// use a tunnel conifigured in ngrok.yml file by providing the tunnel name
var tunnelDetail = await Ragnarok.ConnectAsync(tunnelName); // https://92832de0.ngrok.io -> http://localhost:8888

// specify the tunnel options using the TunnelDefinition
var tunnelDetail = await Ragnarok.ConnectAsync(tunnelDefinition);
```
A few options are available when passing a `TunnelDefinition` to the `Connect` method. Besides constructing an instance, you can utilize the `Action<TunnelDefinition>` override or use the available `extension` methods.
  <details>
    <summary>Examples</summary>  
    
```csharp  
// Action<T>
var tunnelDetail = await Ragnarok.ConnectAsync(opts => 
{ 
    opts.Address = "8443"; 
    opts.Auth = credentials;
    opts.Scheme = Scheme.https;
    opts.Domain = "add-your-static-ngrok-domain";
});

// Fluent builder (using extensions)
var tunnelDetail = await Ragnarok.ConnectAsync(new TunnelDefinition().WithName("myApp")
                                                                     .WithPort(5432)
                                                                     .WithAuthCredentials("bob", "passw0rd"));
```
    
  </details>  
    
> *All `Connect` methods have an optional `authToken` parameter*
</details>  
  
<details>
  <summary>Disconnect</summary>  
  
### Disconnect
Disconnect provides an alterante option for stopping a tunnel. The `tunnel url` is provided as opposed to `tunnel name` which the `StopTunnelAsync()` method requires. Additionally, if a `url` is not provided, **all** open tunnels will be stopped.

```csharp  
await Ragnarok.DisconnectAsync(url); //stop the specified tunnel
await Ragnarok.DisconnectAsync(); //stop all active tunnels
```
</details>
  
# [ngrok.yml](https://ngrok.com/docs#config)
  The `RagnarokClient` respects all properties defined in the [ngrok configuration file](https://ngrok.com/docs#config-default-location). Tunnels defined within the configurations file can be launched by passing the tunnel name to the `Connect` method. Additionally, when launching the `ngrok` client, reference is made to the properties configured. This comes with a few caveats to note:
  * `web_addr: false` - This is not supported as an address is required for the Api to function. 
  * `log_level: info` - This is the preferred `log_level` for the `ngrok` process.
  * `log: stdout` - This is the preferred `log` option, althought `stderr` is also acceptable. 
  * `log: false` | `log: /log/ngrok.log` - Both these options will disable the `Connected`, `Disconnected` and `TunnelCreated` events.
  * `console_ui: true` - Conditional support: if console ui is enabled, `log` cannot be set to `stdout` or `stderr`. 
  
