using Newtonsoft.Json;

namespace CardsHOL.Api.DTOs
{
    public class CardDTO
    {
        [JsonProperty(propertyName: "suit")]
        public string Suit { get; set; }
        [JsonProperty(propertyName: "number")]
        public string Number { get; set; }
    }
}
