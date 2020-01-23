using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace PetfinderAPIWrapper.API.Auth
{
    public class AccessTokenWrapper
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        private long _ExpiresInSeconds;
        [JsonPropertyName("expires_in")]
        public long ExpiresInSeconds
        {
            get
            {
                return _ExpiresInSeconds;
            }
            set
            {
                _ExpiresInSeconds = value;
                ExpiresAtTime = DateTime.Now.AddSeconds(_ExpiresInSeconds);
            }
        }

        public DateTime ExpiresAtTime { get; set; }
    }
}
