using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardsBlazor.Data;
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
                int skip = (queryString.TryGetValue("$skip", out Skip)) ? Convert.ToInt32(Skip[0]) : 0;
                int top = (queryString.TryGetValue("$top", out Take)) ? Convert.ToInt32(Take[0]) : data.Count;
                return new { Items = data.Skip(skip).Take(top), Count = count };
            }
            else
            {
                return data;
            }
        }

        // POST api/<GameController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<GameController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

    }
}
