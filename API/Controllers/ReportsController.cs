using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportsController : ControllerBase
{
    private readonly KutubxonaDbContext _context;

    public ReportsController(KutubxonaDbContext context)
    {
        _context = context;
    }

    [HttpGet("TopBooks")]
    public async Task<IActionResult> GetTopOrderBooks()
    {
        var topBooks = await _context.Books
            .Include(b => b.Orders)
            .OrderByDescending(b => b.Orders.Count())
            .Select(b => new { b.Title, OrderCount = b.Orders.Count() })
            .ToListAsync();

        return Ok(topBooks);
    }
}
