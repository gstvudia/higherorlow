using CardsHOL.Api.DTOs;
using CardsHOL.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CardsHOL.Api.Controllers
{
    [ApiController]

    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        /// <summary>
        /// Start a new game
        /// </summary>
        /// <returns>A game new overview with the deck of cards</returns>
        /// <response code="200">Returns the expected</response>
        [HttpPost("Start")]
        public async Task<IActionResult> Start()
        {
            var response = await _gameService.Start();
            return Ok(response);
        }

        /// <summary>
        /// Guess next card
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id">ID of the game</param>
        /// <returns>A game overview object with the result of the guess</returns>
        /// <response code="200">Returns the expected</response>
        /// <response code="404">Game has already finished</response>  
        [HttpPut("Guess/{id}")]
        public async Task<IActionResult> Guess([FromBody] GameOverviewRequest request, [FromRoute] int id)
        {
            var response = await _gameService.Guess(request.IsHigher, id);

            if(response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }



        /// <summary>
        /// Gets game overview
        /// </summary>
        /// <param name="id">ID of the game</param>
        /// <returns>A game overview object with the result of the guess</returns>
        /// <response code="200">Returns the expected</response>
        /// <response code="404">Game not found</response>  
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOverview( [FromRoute] int id)
        {
            var response = await _gameService.GetOverview(id);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }


        /// <summary>
        /// Checks if the game is finished
        /// </summary>
        /// <param name="id">ID of the game</param>
        /// <returns>Game status</returns>
        /// <response code="200">Game is finished</response>
        /// <response code="404">Game is not finished</response>  
        [HttpHead("{id}")]
        public async Task<IActionResult> isFinished([FromRoute] int id)
        {
            var response = await _gameService.IsFinished(id);

            if (!response)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
