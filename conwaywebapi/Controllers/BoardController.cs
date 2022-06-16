using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ConwayWebApi.Database;
using Microsoft.EntityFrameworkCore;
using ConwayWebModel;

namespace ConwayWebApi.Controllers;

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
  public async Task<ActionResult<IEnumerable<BoardInfo>>> ListBoards()
  {
    return await _context.Boards.Select(b => b.ToBoardInfo()).ToListAsync();
  }

  // Get the contents of a board with a particular ID
  [HttpGet("{id}", Name = nameof(GetBoard))]
  public async Task<ActionResult<BoardDetail>> GetBoard(int id)
  {
    var board = await _context.Boards.Include(b => b.BoardCells).FirstOrDefaultAsync(b => b.ID == id);
    if (board == null)
    {
      return NotFound();
    }

    return board.ToBoardDetail();
  }

  // Put a new board
  [HttpPost]
  public async Task<ActionResult<BoardInfo>> CreateBoard(BoardDetail detail)
  {
    var board = _context.Boards.Add(new Board(detail)).Entity;
    await _context.SaveChangesAsync();
    return CreatedAtAction(
      nameof(GetBoard),
      new { board.ID },
      new BoardInfo(detail.Info, board.ID));
  }

  // Delete a board with a particular ID
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteBoard(int id)
  {
    var board = await _context.Boards.FindAsync(id);

    if (board == null)
    {
      return NotFound();
    }

    _context.Boards.Remove(board);
    await _context.SaveChangesAsync();

    return NoContent();
  }
}