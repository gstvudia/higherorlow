using AutoMapper;
using CardsHOL.Api.Data;
using CardsHOL.Api.DTOs;
using CardsHOL.Api.Entities;
using CardsHOL.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardsHOL.Api.Services
{
    public class GameService
        : IGameService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public GameService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GameOverviewResponse> Start()
        {
            var cards = _context.Cards.ToArray();
            var decks = new List<Deck>();
            Random random = new Random();

            Array.Sort(cards, (a, b) => random.Next(0, 2) == 0 ? -1 : 1);

            var game = await _context.Games.AddAsync(
                    new Game
                    {
                        Round = 1
                    });


            await _context.SaveChangesAsync();

            foreach (var card in cards)
            {
                decks.Add(
                     new Deck
                     {
                         GameId = game.Entity.Id,
                         CardId = card.Id
                     });
            }

            await _context.Decks.AddRangeAsync(decks);
            await _context.SaveChangesAsync();

            return new GameOverviewResponse 
            {
                Card = _mapper.Map<CardDTO>(cards.FirstOrDefault()),
                Round = 1,
                GameId = game.Entity.Id
            };
        }


        public async Task<GameOverviewResponse> Guess(bool isHigher, int gameId)
        {
            bool guessCorrect = false;
            var game = _context.Games.FirstOrDefault(g => g.Id == gameId);

            if(game == null ||
                game.Round == 52)
            {
                return null;
            }

            var currentDeck = (from games in _context.Games.AsNoTracking()
                               join decks in _context.Decks.AsNoTracking()
                                    on games.Id equals decks.GameId
                               join cards in _context.Cards.AsNoTracking()
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

            if ((isHigher && nextCard.Number > currentCard.Number) ||
                (!isHigher && nextCard.Number < currentCard.Number) ||
                nextCard.Suit == currentCard.Suit)
            {
                guessCorrect = true;
            }

            game.Round += 1;
             _context.Games.Update(game);
            await _context.SaveChangesAsync();

            return new GameOverviewResponse
            {
                Card = _mapper.Map<CardDTO>(nextCard),
                Round = game.Round,
                RightGuess = guessCorrect,
                GameId = game.Id
            };
        }

        public async Task<GameOverviewResponse> GetOverview(int gameId)
        {
            var game = _context.Games.FirstOrDefault(g => g.Id == gameId);

            if (game == null)
            {
                return null;
            }

            var currentDeck = (from games in _context.Games.AsNoTracking()
                               join decks in _context.Decks.AsNoTracking() 
                                    on games.Id equals decks.GameId
                               join cards in _context.Cards.AsNoTracking()
                                    on decks.CardId equals cards.Id
                               where decks.GameId == gameId
                               orderby decks.Id ascending
                               select cards)
                         .ToArray();

            var currentCard = currentDeck
                               .Skip(game.Round - 1)
                               .FirstOrDefault();

            return new GameOverviewResponse
            {
                Card = _mapper.Map<CardDTO>(currentCard),
                Round = game.Round,
                GameId = game.Id
            };
        }

        public async Task<bool> IsFinished(int gameId)
        {
            var game = _context.Games
                .AsNoTracking()
                .FirstOrDefault(g => g.Id == gameId);

            if (game == null ||
                game.Round == 52)
            {
                return true;
            }

            return false;
        }
    }
}
