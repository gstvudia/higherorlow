using Newtonsoft.Json;

namespace CardsHOL.Api.DTOs
{
    public class GameOverviewRequest
    {
        [JsonProperty(propertyName: "isHigher")]
        public bool IsHigher { get; set; }
    }
}
