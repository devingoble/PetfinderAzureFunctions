using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PetfinderAPIWrapper
{
    public class AuthStateEntity : IAuthStateEntity
    {
        [JsonPropertyName("token_wrapper")]
        public AccessTokenWrapper TokenWrapper { get; set; }

        public void Set(AccessTokenWrapper accessToken) => TokenWrapper = accessToken;

        public Task<AccessTokenWrapper> Get() => Task.FromResult(TokenWrapper);

        [FunctionName(nameof(AuthStateEntity))]
        public static Task Run([EntityTrigger] IDurableEntityContext ctx)
            => ctx.DispatchAsync<AuthStateEntity>();
    }
}
