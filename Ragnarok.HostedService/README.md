<div align="center">
    <h1> Ragnarok </h1>
</div>

# Ragnarok.HostedService
By adding the `NgrokHostedService` to your application, ngrok tunnels will be created for any local application URLs.

## Installation
Nuget package not yet available

# Usage
## UseNgrok Extension
An extension method is provided on the `IWebHostBuilder` to setup the `NgrokHostedService`. 
Optional parameters are available that allow for setting the ngrok `authtoken` as well as options for managing the bahavior of the `RagnarokClient`.  
> *The option to automatically download ngrok is **disabled** by default. It can be enabled via the options parameter.*
```csharp
public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(x => x
            .UseStartup<Startup>()
            .UseNGrok());
```
## Behind the scenes
On [application startup](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.hosting.iapplicationlifetime.applicationstarted?view=aspnetcore-5.0), all registered [server addresses](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.hosting.server.features.iserveraddressesfeature.addresses?view=aspnetcore-5.0) are retrieved, an `ngrok` client is started (if necessary) and tunnels are dynamically created for each server address.
The `public url` for the created tunnels can be retrieved via the `ngrok Agent Api`, by subscribing to the `Ready` event of the hosted service, or subscribing to the `TunnelCreated` event of the `RagnarokClient`. 

## Ngrok Agent Api
The `NgrokHostedService` is added to the applications `ServiceCollection`, allowing for access via `Dependency Injection`. It provides full access to the [ngrok Agent Api](https://ngrok.com/docs#client-api) via the `RagnarokClient` property.


