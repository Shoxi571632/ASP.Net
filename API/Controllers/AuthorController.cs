using API.Data;
using API.Modul;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly KutubxonaDbContext _context;

        public AuthorController(KutubxonaDbContext context)
        {
            _context = context;
        }

        // GET: api/Author/List
        [HttpGet("List")]
        public async Task<IActionResult> GetAuthors()
        {
            var authors = await _context.Authors
                .Include(a => a.Books)
                .Select(a => new
                {
                    a.Id,
                    a.Name,
                    BookCount = a.Books.Count
                })
                .ToListAsync();

            return Ok(authors);
        }

        // GET: api/Author/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            var author = await _context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author == null)
                return NotFound("Muallif topilmadi.");

            return Ok(author);
        }

        // POST: api/Author
        [HttpPost("Add")]
        public async Task<IActionResult> AddAuthor([FromBody] Author newAuthor)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model ma'lumotlari noto'g'ri.");

            _context.Authors.Add(newAuthor);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAuthorById), new { id = newAuthor.Id }, newAuthor);
        }

        // PUT: api/Author/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, [FromBody] Author updatedAuthor)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model ma'lumotlari noto'g'ri.");

            var existingAuthor = await _context.Authors.FindAsync(id);
            if (existingAuthor == null)
                return NotFound("Muallif topilmadi.");

            existingAuthor.Name = updatedAuthor.Name;
            await _context.SaveChangesAsync();

            return Ok(existingAuthor);
        }

        // DELETE: api/Author/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
                return NotFound("Muallif topilmadi.");

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
