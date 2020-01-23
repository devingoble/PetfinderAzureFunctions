using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetfinderAPIWrapper.API.Auth;

namespace PetfinderAPIWrapper.API
{
    public class EntryPointFunction
    {
        private readonly IPetfinderAuth _petfinderAuth;
        private readonly ApiOptions _options;
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public EntryPointFunction(IHttpClientFactory httpClientFactory, IPetfinderAuth petFinderAuth, IOptions<ApiOptions> options, ILogger log)
        {
            _petfinderAuth = petFinderAuth;
            _options = options.Value;
            _httpClient = httpClientFactory.CreateClient("petfinderapi");
            _logger = log;
        }

        [FunctionName("pets")]
        public async Task<IActionResult> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "pets/{passthroughRoute}")]HttpRequestMessage req,
            [DurableClient] IDurableEntityClient client,
            string passthroughRoute)
        {
            var accessToken = await _petfinderAuth.GetAuthTokenAsync(client);
            var uri = new Uri(Path.Join(_options.PetfinderAPIBaseUrl, passthroughRoute));

            _logger.LogInformation("Making Petfinder call", uri.ToString(), uri, req, client);

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken.AccessToken}");
            var pfDataMessage = await _httpClient.GetAsync(uri);

            if (pfDataMessage.IsSuccessStatusCode)
            {
                var dataString = await pfDataMessage.Content.ReadAsStringAsync();

                _logger.LogInformation("Petfinder reponse", dataString);

                var returnContent = new ContentResult();
                returnContent.Content = dataString;
                returnContent.ContentType = "application/json";
                returnContent.StatusCode = (int)HttpStatusCode.OK;

                return returnContent;
            }

            _logger.LogError("Error from Petfinder", pfDataMessage);

            var code = pfDataMessage.StatusCode == HttpStatusCode.NotFound ? HttpStatusCode.NotFound : HttpStatusCode.InternalServerError;

            return new StatusCodeResult((int)code);
        }
    }
}