using BookShelf.Api.Data;
using BookShelf.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShelf.Api.Repositories;

public class BookRepository : IBookRepository
{
    private readonly LibraryContext _db;
    public BookRepository(LibraryContext db) => _db = db;

    // Получить все книги с авторами
    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _db.Books
            .Include(b => b.Author)   // подтягиваем автора
            .ToListAsync();
    }

    // Получить книгу по Id (с автором)
    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _db.Books
            .Include(b => b.Author)   // подтягиваем автора
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    // Добавить книгу
    public async Task AddAsync(Book book)
    {
        _db.Books.Add(book);
        await _db.SaveChangesAsync();
    }

    // Обновить книгу
    public async Task UpdateAsync(Book book)
    {
        _db.Books.Update(book);
        await _db.SaveChangesAsync();
    }

    // Удалить книгу
    public async Task DeleteAsync(int id)
    {
        var entity = await _db.Books.FindAsync(id);
        if (entity is null) return;   // если нет — ничего не делаем
        _db.Books.Remove(entity);
        await _db.SaveChangesAsync();
    }

    // Найти книги по имени автора
    public async Task<IEnumerable<Book>> GetByAuthorNameAsync(string authorName)
    {
        var query = _db.Books
            .Include(b => b.Author)
            .Where(b => b.Author.Name == authorName);

        /*
           SQL-эквивалент:
           SELECT b.*
           FROM Books b
           JOIN Authors a ON a.Id = b.AuthorId
           WHERE a.Name = @authorName;
        */

        return await query.ToListAsync();
    }
}
