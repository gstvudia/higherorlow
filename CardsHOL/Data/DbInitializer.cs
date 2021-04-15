using CardsHOL.Api.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CardsHOL.Api.Data
{
    public static class DbInitializer
    {
        public async static Task Seed(ApplicationDbContext context)
        {
            await context.Database.MigrateAsync();

            if (!context.Cards.Any())
            {
                context.Cards.AddRange(Card.CardsCollection());
                await context.SaveChangesAsync();
            }
        }
    }
}
