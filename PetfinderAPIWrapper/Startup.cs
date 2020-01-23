using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using PetfinderAPIWrapper.API.Auth;

[assembly: FunctionsStartup(typeof(PetfinderAPIWrapper.Startup))]

namespace PetfinderAPIWrapper
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddOptions<ApiOptions>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.Bind(settings);
                });

            builder.Services.AddHttpClient("petfinderapiauth", client =>
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("PetFinderAPIAuthUrl") ?? "");
            });

            builder.Services.AddHttpClient("petfinderapi", client =>
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("PetFinderAPIBaseUrl") ?? "");
            });

            builder.Services.AddTransient<IPetfinderAuth, PetfinderAuth>();
        }
    }
}