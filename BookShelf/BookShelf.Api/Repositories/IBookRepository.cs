using BookShelf.Api.Models;

namespace BookShelf.Api.Repositories;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(int id);
    Task AddAsync(Book book);
    Task UpdateAsync(Book book);
    Task DeleteAsync(int id);

    Task<IEnumerable<Book>> GetByAuthorNameAsync(string authorName);
}
