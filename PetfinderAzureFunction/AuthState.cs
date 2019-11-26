using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PetfinderAzureFunction
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AuthState
    {
        [JsonProperty("value")]
        public string AccessToken { get; set; }

        public void Set(string token) => this.AccessToken = token;

        public string Get() => this.AccessToken;

        [FunctionName(nameof(AuthState))]
        public static Task Run([EntityTrigger] IDurableEntityContext ctx)
            => ctx.DispatchAsync<AuthState>();
    }
}
