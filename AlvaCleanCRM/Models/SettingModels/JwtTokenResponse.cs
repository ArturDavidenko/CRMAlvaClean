using System.Text.Json.Serialization;

namespace AlvaCleanCRM.Models.SettingModels
{
    public class JwtTokenResponse
    {
        [JsonPropertyName("token")]
        public string token { get; set; }
    }
}
