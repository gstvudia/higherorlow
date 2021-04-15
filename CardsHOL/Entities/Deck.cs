using CardsHOL.Api.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardsHOL.Api.Entities
{
    public class Deck
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int CardId { get; set; }
        public int GameId { get; set; }

        #region Relations
        public Card Card { get; set; }
        public Game Game { get; set; }
        #endregion
    }
}
