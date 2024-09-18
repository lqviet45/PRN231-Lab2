using lab2.common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace lab2.Controllers;

public class PressesController : ODataController
{
    private readonly BookStoreDbContext _context;

    public PressesController(BookStoreDbContext context)
    {
        _context = context;
        
        if (!_context.Presses.Any())
        {
            foreach (var book in DataSource.GetBooks())
            {
                _context.Books.Add(book);
                _context.Presses.Add(book.Press);
            }
            
            _context.SaveChanges();
        }
    }
    
    [EnableQuery]
    public async Task<IActionResult> Get()
    {
        var presses = await _context.Presses.ToListAsync();
        return Ok(presses);
    }
}