using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CardsBlazor.Data;
using CardsBlazor.Data.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace CardsBlazor.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private PlayerService _playerService;
        public PlayerController(PlayerService service)
        {
            _playerService = service;
        }

        public object Get()
        {
            var data = _playerService.GetAllAsQueryable();
            var count = data.Count();
            var queryString = Request.Query;
            if (queryString.Keys.Contains("$inlinecount"))
            {
                StringValues Skip;
                StringValues Take;
                int skip = (queryString.TryGetValue("$skip", out Skip)) ? Convert.ToInt32(Skip[0], CultureInfo.InvariantCulture) : 0;
                int top = (queryString.TryGetValue("$top", out Take)) ? Convert.ToInt32(Take[0], CultureInfo.InvariantCulture) : data.Count();
                return new { Items = data.Skip(skip).Take(top), Count = count };
            }
            else
            {
                return data;
            }
        }

        [HttpPost]
        public async void Post([FromBody] PlayerViewModel player)
        {
            await Task.Delay(0);
            _playerService.AddPlayer(player);
        }

        [Route("Position")]
        [HttpGet]
        public async Task<IActionResult> GetPositionGraph(int playerId)
        {
            await Task.Delay(0);
            return Ok(_playerService.GetPositionGraphClasses(playerId, 7));
        }
    }
}
