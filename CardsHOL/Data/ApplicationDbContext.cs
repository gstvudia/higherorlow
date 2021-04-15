using CardsHOL.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace CardsHOL.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Card> Cards { get; set; }
        public DbSet<Deck> Decks { get; set; }
        public DbSet<Game> Games { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .Entity<Deck>()
                .HasOne(e => e.Card)
                .WithMany(s => s.Decks)
                .HasForeignKey(e => e.CardId);

            builder
                .Entity<Deck>()
                .HasOne(e => e.Game)
                .WithMany(s => s.Decks)
                .HasForeignKey(e => e.GameId);
        }
    }
}
