using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CardsBlazor.Data;
using CardsBlazor.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CardsBlazor.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private GameService _gameService;
        public GameController(GameService service)
        {
            _gameService = service;
        }
        // GET: api/<GameController>
        [HttpGet]
        public object Get()
        {
            var data = _gameService.GetAllAsViewModels().Result;
            var count = data.Count;
            var queryString = Request.Query;
            if (queryString.Keys.Contains("$inlinecount"))
            {
                StringValues Skip;
                StringValues Take;
                int skip = (queryString.TryGetValue("$skip", out Skip)) ? Convert.ToInt32(Skip[0], CultureInfo.InvariantCulture) : 0;
                int top = (queryString.TryGetValue("$top", out Take)) ? Convert.ToInt32(Take[0], CultureInfo.InvariantCulture) : data.Count;
                return new { Items = data.Skip(skip).Take(top), Count = count };
            }
            else
            {
                return data;
            }
        }

        /// <summary>
        /// Retrieves a list of all games from the database
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public List<GameViewModel> GetAll()
        {
            return _gameService.GetAllAsViewModels().Result;
        }
    }
}
