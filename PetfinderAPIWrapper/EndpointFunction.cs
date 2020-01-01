using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace PetfinderAPIWrapper
{
    public class EndpointFunction
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IKeyVaultClient _keyVaultClient;

        public EndpointFunction(IHttpClientFactory clientFactory, IKeyVaultClient keyVaultClient)
        {
            _httpClientFactory = clientFactory;
            _keyVaultClient = keyVaultClient;
        }

        [FunctionName("Endpoint_HttpStart")]
        public async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")]HttpRequestMessage req,
            [DurableClient] IDurableEntityClient client,
            ILogger log)
        {
            var entityId = new EntityId(nameof(AuthStateEntity), "petfinderauth");
            var response = await client.ReadEntityStateAsync<IAuthStateEntity>(entityId);
            AccessTokenWrapper accessToken;

            if (!response.EntityExists)
            {
                var token = await Authenticate();
                response.EntityState.Set(token);
                accessToken = token;
            }
            else
            {
                accessToken = await response.EntityState.Get();
            }

            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        }

        private async Task<AccessTokenWrapper> Authenticate()
        {
            var client = _httpClientFactory.CreateClient("petfinderapiauth");

            var requestData = new List<KeyValuePair<string, string>>();
            requestData.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));

            var requestBody = new FormUrlEncodedContent(requestData);

            var request = await client.PostAsync(Environment.GetEnvironmentVariable("Values:PetFinder-API-Url"), requestBody);
            var response = await request.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<AccessTokenWrapper>(response);

        }
    }
}