using BookShelf.Api.Data;
using BookShelf.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShelf.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly LibraryContext _db;
    public AuthorsController(LibraryContext db) => _db = db;

    // GET /api/Authors
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _db.Authors.AsNoTracking().ToListAsync());

    // GET /api/Authors/id
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var author = await _db.Authors.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        return author is null ? NotFound() : Ok(author);
    }

    // POST /api/Authors
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Author author)
    {
        if (string.IsNullOrWhiteSpace(author.Name))
            return BadRequest("Name is required");

        _db.Authors.Add(author);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = author.Id }, author);
    }

    // PUT /api/Authors/id
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Author author)
    {
        if (id != author.Id) return BadRequest("Id mismatch");
        var exists = await _db.Authors.AnyAsync(a => a.Id == id);
        if (!exists) return NotFound();

        _db.Entry(author).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE /api/Authors/id
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var author = await _db.Authors.FindAsync(id);
        if (author is null) return NoContent();

        _db.Authors.Remove(author);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
