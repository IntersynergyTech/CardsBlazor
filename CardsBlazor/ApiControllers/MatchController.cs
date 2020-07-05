using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardsBlazor.Data;
using CardsBlazor.Data.Entity;
using CardsBlazor.Data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CardsBlazor.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly MatchService _service;
        public MatchController(MatchService matchService)
        {
            _service = matchService;
        }

        public ActionResult<MatchViewModel> Get()
        {
            return null;
        }  
    }
}
