using lab2.common;
using lab2.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace lab2.Controllers;

public class BooksController : ODataController
{
    private readonly BookStoreDbContext _context;

    public BooksController(BookStoreDbContext context)
    {
        _context = context;
        _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

        if (!_context.Books.Any())
        {
            foreach (var book in DataSource.GetBooks())
            {
                _context.Books.Add(book);
                _context.Presses.Add(book.Press);
            }
            
            _context.SaveChanges();
        }
    }

    [EnableQuery(PageSize = 10)]
    public async Task<IActionResult> Get()
    {
        var books = await _context.Books
            .Include(b => b.Press)
            .ToListAsync();
        return Ok(books);
    }

    [EnableQuery]
    public async Task<IActionResult> Get(int key, string version)
    {
        var book = await _context.Books
            .Include(b => b.Press)
            .FirstOrDefaultAsync(b => b.Id == key);
        
        if (book == null)
        {
            return NotFound();
        }
        
        return Ok(book);
    }
    
    [EnableQuery]
    public async Task<IActionResult> Post([FromBody] Book book)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        return Created(book);
    }
    
    [EnableQuery]
    public async Task<IActionResult> Delete(int key)
    {
        var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == key);
        
        if (book == null)
        {
            return NotFound();
        }

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}