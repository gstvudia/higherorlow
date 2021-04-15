using AutoMapper;
using CardsHOL.Api.Data;
using CardsHOL.Api.DTOs;
using CardsHOL.Api.Entities;
using CardsHOL.Api.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CardsHOL.Tests
{
    public class Tests
    {
        [Test]
        public async Task StartGame_Sucess()
        {
            GameOverviewResponse response;

            #region Setup && Execution
            Mock<IMapper> mapper = new Mock<IMapper>();
            using (var context = GetContext())
            {
                await context.Cards.AddRangeAsync(Card.CardsCollection());
                await context.SaveChangesAsync();

                mapper.Setup(m => m.Map<CardDTO>(It.IsAny<Card>())).Returns(new CardDTO { });

                var service = new GameService(context, mapper.Object);
                response = await service.Start();


                #region Tests
                Assert.True(context.Decks.GroupBy(d => d.CardId).All(c => c.Count() == 1));
                Assert.NotNull(response);
                Assert.NotNull(response.Card);
                Assert.NotNull(context.Decks.Select(d => d));
                Assert.NotNull(context.Games.Select(g => g));
                Assert.True(context.Decks.Count() == 52);
                #endregion
            }
            #endregion
        }


        [Test]
        public async Task GuessCard_Correct()
        {
            GameOverviewResponse response;
            Mock<IMapper> mapper = new Mock<IMapper>();
            #region Setup && Execution
            using (var context = GetContext())
            {
                await context.Cards.AddRangeAsync(Card.CardsCollection());
                await context.SaveChangesAsync();

                mapper.Setup(m => m.Map<CardDTO>(It.IsAny<Card>())).Returns(new CardDTO { });

                var service = new GameService(context, mapper.Object);
                var game = await service.Start();
                Assert.NotNull(game);
                int gameId = context.Games.FirstOrDefault().Id;
                bool guess = false;

                var currentDeck = (from games in context.Games.AsNoTracking()
                                   join decks in context.Decks.AsNoTracking() 
                                        on games.Id equals decks.GameId
                                   join cards in context.Cards.AsNoTracking() 
                                        on decks.CardId equals cards.Id
                                   where decks.GameId == gameId
                                   orderby decks.Id ascending
                                   select cards)
                         .ToArray();

                var currentCard = currentDeck
                                   .Skip(game.Round - 1)
                                   .FirstOrDefault();

                var nextCard = currentDeck
                                   .Skip(game.Round)
                                   .FirstOrDefault();

                guess = nextCard.Number > currentCard.Number ? true : false;
                context.ChangeTracker.Entries()
                    .Where(e => e.Entity.GetType() == typeof(Game))
                    .FirstOrDefault()
                    .State = EntityState.Detached;
                    
                response = await service.Guess(guess, gameId);


                #region Tests
                Assert.NotNull(response);
                Assert.NotNull(context.Games.FirstOrDefault(g => g.Round > 1));
                Assert.True(response.RightGuess);
                #endregion
            }
            #endregion
        }


        [Test]
        public async Task GuessCard_Incorrect()
        {
            GameOverviewResponse response;
            Mock<IMapper> mapper = new Mock<IMapper>();
            #region Setup && Execution
            using (var context = GetContext())
            {
                await context.Cards.AddRangeAsync(Card.CardsCollection());
                await context.SaveChangesAsync();

                mapper.Setup(m => m.Map<CardDTO>(It.IsAny<Card>())).Returns(new CardDTO { });

                var service = new GameService(context, mapper.Object);
                var game = await service.Start();
                Assert.NotNull(game);
                int gameId = context.Games.FirstOrDefault().Id;
                bool guess = false;

                var currentDeck = (from games in context.Games.AsNoTracking()
                                   join decks in context.Decks.AsNoTracking()
                                        on games.Id equals decks.GameId
                                   join cards in context.Cards.AsNoTracking()
                                        on decks.CardId equals cards.Id
                                   where decks.GameId == gameId
                                   orderby decks.Id ascending
                                   select cards)
                         .ToArray();

                var currentCard = currentDeck
                                   .Skip(game.Round - 1)
                                   .FirstOrDefault();

                var nextCard = currentDeck
                                   .Skip(game.Round)
                                   .FirstOrDefault();

                guess = nextCard.Number > currentCard.Number ? false : true;
                context.ChangeTracker.Entries()
                    .Where(e => e.Entity.GetType() == typeof(Game))
                    .FirstOrDefault()
                    .State = EntityState.Detached;

                response = await service.Guess(guess, gameId);


                #region Tests
                Assert.NotNull(response);
                Assert.NotNull(context.Games.FirstOrDefault(g => g.Round > 1));
                Assert.IsFalse(response.RightGuess);
                #endregion
            }
            #endregion
        }



        public ApplicationDbContext GetContext()
        {
            
            var dbGUID = Guid.NewGuid();
            var dbName = $"Cards_{dbGUID}";
            //Ensure one memory database peer unit test, ensuring concurency
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"DB_{dbName}")
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .EnableSensitiveDataLogging()
                .Options;

            return new ApplicationDbContext(options);
        }
    }
}