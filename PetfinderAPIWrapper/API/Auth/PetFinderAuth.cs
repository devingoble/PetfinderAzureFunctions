using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PetfinderAPIWrapper.API.Auth
{
    public class PetfinderAuth : IPetfinderAuth
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiOptions _options;
        private readonly ILogger<PetfinderAuth> _logger;

        public PetfinderAuth(IHttpClientFactory httpClientFactory, IOptions<ApiOptions> options, ILogger<PetfinderAuth> log)
        {
            _httpClientFactory = httpClientFactory;
            _options = options.Value;
            _logger = log;
        }

        public async Task<AccessTokenWrapper> GetAuthTokenAsync(IDurableEntityClient client)
        {
            _logger.LogInformation($"Auth URL: {_options.PetfinderAPIAuthUrl}");

            var entityId = new EntityId(nameof(AuthStateEntity), "petfinderauth");
            var response = await client.ReadEntityStateAsync<AuthStateEntity>(entityId);
            AccessTokenWrapper accessToken;

            if (response.EntityExists)
            {
                _logger.LogInformation("Reading existing entity");
                accessToken = await response.EntityState.Get();
            }
            else
            {
                _logger.LogInformation("New authentication");
                accessToken = await Authenticate();
            }

            _logger.LogInformation($"Access Token Expiration - Expiration Time: {accessToken.ExpiresAtTime} - Expiration Duration: {accessToken.ExpiresInSeconds}");

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

    }
}
