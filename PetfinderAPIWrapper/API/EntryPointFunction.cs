using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
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
        private readonly HttpClient _httpClientApiWrapper;
        private readonly ILogger<EntryPointFunction> _logger;

        public EntryPointFunction(IHttpClientFactory httpClientFactory, IPetfinderAuth petFinderAuth, IOptions<ApiOptions> options, ILogger<EntryPointFunction> log)
        {
            _petfinderAuth = petFinderAuth;
            _options = options.Value;
            _httpClient = httpClientFactory.CreateClient("petfinderapi");
            _httpClientApiWrapper = httpClientFactory.CreateClient("wrapperapi");
            _logger = log;
        }

        [FunctionName("pets")]
        public async Task<IActionResult> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "{**passthroughRoute}")]HttpRequestMessage req,
            [DurableClient] IDurableEntityClient client,
            string passthroughRoute)
        {
            var accessToken = await _petfinderAuth.GetAuthTokenAsync(client);
            var query = req.RequestUri.Query;
            var builder = new UriBuilder(Path.Join(_options.PetfinderAPIBaseUrl, passthroughRoute));
            builder.Query = query;
            var uri = builder.Uri;

            _logger.LogInformation($"Making Petfinder call: URI: {uri.ToString()}, HttpRequestMessage: {req.RequestUri.ToString()}, Client: {client.TaskHubName}");
            
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken.AccessToken}");
            var pfDataMessage = await _httpClient.GetAsync(uri);

            if (pfDataMessage.IsSuccessStatusCode)
            {
                var dataString = await pfDataMessage.Content.ReadAsStringAsync();

                var returnContent = new ContentResult();
                returnContent.Content = dataString;
                returnContent.ContentType = "application/json";
                returnContent.StatusCode = (int)HttpStatusCode.OK;

                return returnContent;
            }

            _logger.LogError($"Error from Petfinder: {await pfDataMessage.Content.ReadAsStringAsync()}");

            var code = pfDataMessage.StatusCode == HttpStatusCode.NotFound ? HttpStatusCode.NotFound : HttpStatusCode.InternalServerError;

            return new StatusCodeResult((int)code);
        }

        [FunctionName("KeepAlive")]
        public async Task TimerStart([TimerTrigger("0 */15 * * * *")]TimerInfo timer)
        {
            _logger.LogDebug("Warmup trigger called");

            var response = await _httpClientApiWrapper.GetAsync("/");

            if (response.IsSuccessStatusCode)
            {
                _logger.LogDebug("Warmup successful");
            }
            else
            {
                _logger.LogError($"Could not warm up API wrapper: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }
    }
}