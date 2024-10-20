using API.Data;
using API.Modul;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookController : ControllerBase
{
    private readonly KutubxonaDbContext _context;

    public BookController(KutubxonaDbContext kutubxona)
    {
        _context = kutubxona;
    }

    // GET: api/Book/List
    [HttpGet("List")]
    public async Task<IActionResult> GetBooks()
    {
        var books = await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .Select(b => new
            {
                b.Id,
                b.Title,
                AuthorName = b.Author.Name,
                GenreName = b.Genre.Name
            })
            .ToListAsync();

        return Ok(books); // To'g'ridan-to'g'ri JSON qaytariladi
    }

    // POST: api/Book
    [HttpPost("Add")]
    public async Task<IActionResult> AddBook([FromBody] Book newBook)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest("Model ma'lumotlari noto'g'ri.");

            // Yangi kitob yaratish
            var book = new Book
            {
                Title = newBook.Title,
                AuthorId = newBook.AuthorId,
                GenreId = newBook.GenreId
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBooks), new { id = book.Id }, book); // Yaratilgan kitob haqida ma'lumot qaytariladi
        }
        catch (Exception ex)
        {
            return BadRequest($"Xatolik yuz berdi: {ex.Message}");
        }
    }

    // PUT: api/Book/5
    [HttpPut("new")]
    public async Task<IActionResult> UpdateBook(int id, [FromBody] Book book)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model ma'lumotlari noto'g'ri.");

        var existingBook = await _context.Books.FindAsync(id);
        if (existingBook == null)
            return NotFound("Kitob topilmadi.");

        // Kitob ma'lumotlarini yangilash
        existingBook.Title = book.Title;
        existingBook.AuthorId = book.AuthorId;
        existingBook.GenreId = book.GenreId;

        await _context.SaveChangesAsync();
        return Ok(existingBook);
    }

    // DELETE: api/Book/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
            return NotFound("Kitob topilmadi.");

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return NoContent(); // O'chirilganini tasdiqlovchi bo'sh javob
    }
}
