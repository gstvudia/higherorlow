using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardsHOL.Api.Entities
{
    public class Card
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Number { get; set; }
        public string Suit { get; set; }

        #region Relations
        public ICollection<Deck> Decks { get; set; }
        #endregion

        #region Get Cards
        public static IEnumerable<Card> CardsCollection()
        {
            var cards = new List<Card>();

            for (int i = 1; i <= 13; i++)
            {
                cards.AddRange(
                    new List<Card>
                    {
                        new Card
                        {
                            Suit = "Clubs",
                            Number = i
                        },
                        new Card
                        {
                            Suit = "Diamonds",
                            Number = i
                        },
                        new Card
                        {
                            Suit = "Hearts",
                            Number = i
                        },
                        new Card
                        {
                            Suit = "Spades",
                            Number = i
                        }
                    });
            };

            return cards;
        }
        #endregion
    }
}
