using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardsBlazor.Data;
using CardsBlazor.Data.Entity;
using CardsBlazor.Data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;

namespace CardsBlazor.ApiControllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
    public class MatchController : ControllerBase
    {
        private readonly MatchService _service;
        private readonly ILogger<MatchController> _logger;
        public MatchController(MatchService matchService, ILogger<MatchController> logger)
        {
            _service = matchService;
            _logger = logger;
        }

        /// <summary>
        /// Returns the specified number of matches in order of start time. Unresolved matches will always be at the start
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take">Defaults to 50</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllMatches")]
        public List<MatchViewModel> GetAll(int skip = 0, int take = 50)
        {
            return _service.GetAllMatches().OrderBy(x => x.IsResolved).ThenByDescending(x => x.StartTime).Skip(skip).Take(take).Select(x => new MatchViewModel(x)).ToList();;
        }
        
        [HttpGet]
        [Route("GetMatch")]
        public IActionResult Get(int matchId)
        {
            var match = _service.GetMatch(matchId);
            if (match == null) return NotFound();
            return Ok(match);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="playerResults"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("FinishMatch")]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ApiKey]
        public IActionResult FinishMatch(int matchId, [FromBody] Dictionary<int, decimal> playerResults)
        {
            var match = _service.GetMatch(matchId);
            if (match == null) return NotFound();
            if (playerResults == null) return BadRequest();
            if (match.Game.NumberOfWinners == NumberOfWinners.SingleWinner)
            {
                var winningPlayerId = playerResults.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                try
                {
                    _service.ResolveSingleWinnerMatch(match.MatchId, winningPlayerId);
                }
                catch (MatchNotFoundException)
                {
                    return NotFound();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to mark match as complete - API Request - {MatchId}", matchId);
                    Response.StatusCode = 500;
                    return new JsonResult("");
                    throw;
                }
               
            }
            else
            {
                _service.ResolveMultiWinnerMatch(match.MatchId, playerResults);
            }

            var updatedMatch = _service.GetMatch(matchId);
            return new JsonResult(updatedMatch);
        }
        
        [HttpPost]
        [Route("StartMatch")]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ApiKey]
        public IActionResult StartMatch([FromBody] MatchCreateModel model)
        {
            if (model == null) return BadRequest("Model is missing");
            if (model.GameId is null or 0) return BadRequest("Invalid Game");
            var matchId = _service.AddNewMatch(model);
            var match = _service.GetMatch(matchId);
            return Ok(match);
        }
    }
}