using Alten.Academy.Jumpstart.Battleship;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Collections.Generic;
using Alten.Academy.Jumpstart.Battleship.Console;
using ConsoleWrapper = Alten.Academy.Jumpstart.Battleship.Console.Console;
using BattleShipAPI.Data;

namespace BattleShipAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BattleShipController : ControllerBase
    {
        private readonly IStateService stateService;
        public BattleShipController(IStateService stateService) 
        {
            this.stateService = stateService;
        }

        [HttpGet("/youWin")]
        public IActionResult win()
        {
            var obj = stateService.getObject();
            obj.MyTurn = false;
            obj.IsGameWon = true;
            obj.MustNotifyOpponent = false;
            return Ok();
        }

        [HttpGet("/youLose")]
        public IActionResult lose()
        {
            var obj = stateService.getObject();
            obj.MyTurn = false;
            obj.IsGameLost = true;
            obj.MustNotifyOpponent = false;
            return Ok();
        }

        [HttpPost("/move/{move}")]
        public IActionResult move(string move, [FromForm] string grid)
        {
            var obj = stateService.getObject();
            var gameText = obj.PlayTurn(move);
            obj.OpponentGrid = grid;
            if (!(obj.IsGameWon || obj.IsGameLost || obj.IsGameCrashed))
            {
                obj.printGame();
            }
            return Ok(gameText);
        }
    }
}