using BookShelf.Api.Models;
using BookShelf.Api.Repositories;
using BookShelf.Api.Services;          //  для сервиса статистики
using Microsoft.AspNetCore.Mvc;

namespace BookShelf.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookRepository _repo;
    private readonly BookService _service;   //  сервис для сырого SQL

    public BooksController(IBookRepository repo, BookService service)
    {
        _repo = repo;
        _service = service;
    }

    // GET /api/books
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var books = await _repo.GetAllAsync();
        return Ok(books);
    }

    // GET /api/books/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var book = await _repo.GetByIdAsync(id);
        return book is null ? NotFound() : Ok(book);
    }

    // POST /api/books
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Book book)
    {
        if (string.IsNullOrWhiteSpace(book.Title))
            return BadRequest("Title is required.");
        if (book.AuthorId <= 0)
            return BadRequest("AuthorId is required.");

        await _repo.AddAsync(book);
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }

    // PUT /api/books/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Book book)
    {
        if (id != book.Id)
            return BadRequest("Id in path and body must match.");

        await _repo.UpdateAsync(book);
        return NoContent();
    }

    // DELETE /api/books/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repo.DeleteAsync(id);
        return NoContent();
    }

    // GET /api/books/by-author?name=...
    [HttpGet("by-author")]
    public async Task<IActionResult> GetByAuthor([FromQuery] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return BadRequest("Author name is required.");

        var books = await _repo.GetByAuthorNameAsync(name);
        return Ok(books);
    }

    // GET /api/books/statistics
    [HttpGet("statistics")]
    public async Task<IActionResult> GetStatistics()
    {
        var stats = await _service.GetStatisticsAsync();
        return Ok(stats);
    }
}
