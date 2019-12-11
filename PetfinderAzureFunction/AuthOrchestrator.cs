using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace PetfinderAzureFunction
{
    public static class AuthOrchestrator
    {
        static HttpClient _httpClient;
        static IKeyVaultClient _keyVaultClient;

        [FunctionName("AuthOrchestrator")]
        public static async Task<string> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var entityId = new EntityId(nameof(AuthState), "petfinderAuth");
            var proxy = context.CreateEntityProxy<IAuthState>(entityId);
            var authToken = proxy.Get();

            if (string.IsNullOrWhiteSpace(authToken))
            {
                authToken = await context.CallActivityAsync<string>("AuthOrchestrator_Authenticate", )
            }


            var outputs = new List<string>();

            // Replace "hello" with the name of your Durable Activity Function.
            outputs.Add(await context.CallActivityAsync<string>("AuthOrchestrator_Authenticate", "Tokyo"));
            outputs.Add(await context.CallActivityAsync<string>("AuthOrchestrator_Authenticate", "Seattle"));
            outputs.Add(await context.CallActivityAsync<string>("AuthOrchestrator_Authenticate", "London"));

            // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
            return outputs;
        }

        [FunctionName("AuthOrchestrator_Authenticate")]
        public static string Authenticate([ActivityTrigger] string name, ILogger log)
        {

            log.LogInformation($"Saying hello to {name}.");
            return $"Hello {name}!";
        }

        [FunctionName("AuthOrchestrator_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")]HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log,
            IHttpClientFactory httpClientFactory,
            IKeyVaultClient keyVaultClient)
        {
            _keyVaultClient = keyVaultClient;
            _httpClient = httpClientFactory.CreateClient();

            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("AuthOrchestrator", null);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}