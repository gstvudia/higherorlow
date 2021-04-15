using CardsHOL.Api.DTOs;
using System.Threading.Tasks;

namespace CardsHOL.Api.Services.Interfaces
{
    public interface IGameService
    {
        Task<GameOverviewResponse> GetOverview(int gameId);
        Task<GameOverviewResponse> Guess(bool isHigher, int gameId);
        Task<bool> IsFinished(int gameId);
        public Task<GameOverviewResponse> Start();
    }
}
