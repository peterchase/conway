using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ConwayWebApi.Models;

namespace ConwayWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BoardController : ControllerBase
    {
        private readonly ILogger<BoardController> _logger;

        public BoardController(ILogger<BoardController> logger)
        {
            _logger = logger;
        }

        // Get all the boards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BoardInfo>>> Get()
        {
            await Task.Delay(10); // TODO remove fake delay
            return Array.Empty<BoardInfo>();
        }

        // Get the contents of a board with a particular ID
        [HttpGet("{id}")]
        public async Task<ActionResult<BoardDetail>> Get(int id)
        {
            await Task.Delay(10); // TODO remove fake delay
            return NoContent();
        }

        // Post a new board
        [HttpPost]
        public async Task<ActionResult<BoardInfo>> CreateBoard(BoardDetail detail)
        {
            await Task.Delay(10); // TODO remove fake delay
            int fakeId = 123;
            return CreatedAtAction(nameof(BoardDetail), new { id = fakeId }, detail.Info);
        }

        // Delete a board with a particular ID
        [HttpDelete]
        public async Task<IActionResult> DeleteBoard(int id)
        {
            await Task.Delay(10); // TODO remove fake delay
            return NotFound();
        }
    }
}
