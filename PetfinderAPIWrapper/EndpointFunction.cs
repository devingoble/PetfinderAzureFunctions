using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace PetfinderAPIWrapper
{
    public class EndpointFunction
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly APIOptions _options;

        public EndpointFunction(IHttpClientFactory clientFactory, IOptions<APIOptions> options)
        {
            _httpClientFactory = clientFactory;
            _options = options.Value;
        }

        [FunctionName("Endpoint_HttpStart")]
        public async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "pets")]HttpRequestMessage req,
            [DurableClient] IDurableEntityClient client,
            ILogger log)
        {
            var accessToken = await GetAuthToken(client);




            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        }

        private async Task<AccessTokenWrapper> GetAuthToken(IDurableEntityClient client)
        {
            var entityId = new EntityId(nameof(AuthStateEntity), "petfinderauth");
            var response = await client.ReadEntityStateAsync<AuthStateEntity>(entityId);
            AccessTokenWrapper accessToken;

            if (response.EntityExists)
            {
                accessToken = await response.EntityState.Get();
            }
            else
            {
                accessToken = await Authenticate();
            }

            if (accessToken.ExpiresAtTime.AddMinutes(-10) <= DateTime.Now)
            {
                accessToken = await Authenticate();
            }

            await client.SignalEntityAsync<IAuthStateEntity>(entityId, proxy => proxy.Set(accessToken));

            return accessToken;
        }

        private async Task<AccessTokenWrapper> Authenticate()
        {
            var client = _httpClientFactory.CreateClient("petfinderapiauth");

            var requestData = new List<KeyValuePair<string, string>>();
            requestData.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));

            requestData.Add(new KeyValuePair<string, string>("client_id", _options.PetfinderAPIKey));
            requestData.Add(new KeyValuePair<string, string>("client_secret", _options.PetfinderAPISecret));

            var requestBody = new FormUrlEncodedContent(requestData);

            var request = await client.PostAsync(_options.PetfinderAPIAuthUrl, requestBody);
            var response = await request.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<AccessTokenWrapper>(response);
        }

        private async Task<>
    }
}