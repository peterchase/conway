using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ConwayWebApi.Models;
using ConwayWebApi.Database;
using Microsoft.EntityFrameworkCore;

namespace ConwayWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BoardController : ControllerBase
    {
        private readonly ILogger<BoardController> _logger;
        private readonly ConwayContext _context;

        public BoardController(ILogger<BoardController> logger, ConwayContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Get all the boards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BoardInfo>>> Get()
        {
            return await _context.Boards.Select(b => new BoardInfo(b, b.ID)).ToListAsync();
        }

        // Get the contents of a board with a particular ID
        [HttpGet("{id}")]
        public async Task<ActionResult<BoardDetail>> Get(int id)
        {
            var board = await _context.Boards.Include(b => b.BoardCells).FirstOrDefaultAsync(b => b.ID == id);
            if (board == null)
            {
                return NotFound();
            }

            return new BoardDetail(board, id);
        }

        // Put a new board
        [HttpPut]
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
