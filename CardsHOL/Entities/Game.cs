using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardsHOL.Api.Entities
{
    public class Game
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Round { get; set; }

        #region Relations
        public ICollection<Deck> Decks { get; set; }
        #endregion
    }
}
