﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ragnarok.AgentApi;
using Ragnarok.AgentApi.Models;
using Ragnarok.HostedService.Contracts;
using Ragnarok.HostedService.Models;
using System;

namespace Ragnarok.HostedService.Extensions
{
    public static class IWebHostBuilderExtensions
    {
        /// <summary>
        /// Use the <see cref="NgrokHostedService"/>
        /// </summary>
        /// <param name="options">Options to modify the behaviour of the <see cref="RagnarokClient"/></param>
        /// <param name="authToken">The ngrok authToken to register in the ngrok.yml configuration file</param>
        /// <remarks>
        /// Associates the application with an ngrok client process and automatically creates tunnels for all local urls
        /// </remarks>
        public static IWebHostBuilder UseNgrok(this IWebHostBuilder builder, Action<RagnarokOptions> options = null, string authToken = null)
        {
            return builder
                .ConfigureServices((context, services) =>
                {
                    if (authToken != null)
                        services.AddSingleton(new AuthToken(authToken));

                    if (options != null)
                        services.AddOptions<RagnarokOptions>().Configure(options);

                    services.AddLogging();
                    services.AddHttpClient<RagnarokClient>();
                    services.TryAddSingleton<NgrokHostedService>();
                    services.AddHostedService(provider => provider.GetRequiredService<NgrokHostedService>());
                    services.AddSingleton<INgrokHostedService>(provider => provider.GetRequiredService<NgrokHostedService>());
                });
        }
    }
}