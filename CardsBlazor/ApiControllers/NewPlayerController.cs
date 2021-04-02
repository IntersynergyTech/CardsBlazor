using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CardsBlazor.Data;
using CardsBlazor.Data.Entity;
using CardsBlazor.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CardsBlazor.ApiControllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
    public class NewPlayerController : ControllerBase
    {
        private readonly PlayerService _playerService;
        private readonly BoardService _boardService;
        public NewPlayerController(PlayerService service, BoardService boardService)
        {
            _playerService = service;
            _boardService = boardService;
        }
        [HttpGet]
        [MapToApiVersion("2.0")]
        public object Get()
        {
            var data = _playerService.GetAllAsQueryable().AsSplitQuery().Where(x => !x.HideFromView).ToList().Select(x => new
            {
                Id = x.PlayerId,
                UserName = x.UserName,
                RealName = x.RealName,
                CurrentPosition = x.CurrentPosition,
                PotentialPosition = x.PotentialPosition,
                HideFromView = x.HideFromView
            });
            return data;
        }
        [HttpGet]
        [Route("GetBoard")]
        [MapToApiVersion("2.0")]
        public JsonResult GetBoard(int pageSize, int startPos)
        {
            var model = _boardService.GetNPositionsForApi(pageSize, startPos);
            return new(model);
        }
    }
}