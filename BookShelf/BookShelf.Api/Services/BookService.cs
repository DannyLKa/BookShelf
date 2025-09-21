using BookShelf.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace BookShelf.Api.Services;

public record BookCounters(int Read, int Unread);

public class BookService
{
    private readonly LibraryContext _db;
    public BookService(LibraryContext db) => _db = db;

    public async Task<BookCounters> GetStatisticsAsync()
    {
        var read = await _db.Books.CountAsync(x => x.IsRead);
        var unread = await _db.Books.CountAsync(x => !x.IsRead);
        return new BookCounters(read, unread);
    }
}
