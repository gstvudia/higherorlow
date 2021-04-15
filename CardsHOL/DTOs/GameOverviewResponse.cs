using CardsHOL.Api.Entities;
using Newtonsoft.Json;

namespace CardsHOL.Api.DTOs
{
    public class GameOverviewResponse
    {
        [JsonProperty(propertyName: "card")]
        public CardDTO Card { get; set; }
        [JsonProperty(propertyName: "round")]
        public int Round{ get; set; }
        [JsonProperty(propertyName: "rightGuess")]
        public bool? RightGuess { get; set; }
        [JsonProperty(propertyName: "gameId")]
        public int GameId { get; set; }
    }
}
