using BookShelf.Api.Models;
using BookShelf.Api.Repositories;
using Xunit;

namespace BookShelf.Tests;

public class BookRepositoryTests
{
    [Fact]
    public async Task Add_and_get_by_id_returns_book_with_author()
    {
        var (conn, ctx) = TestSqliteFactory.CreateContext();
        try
        {
            var author = new Author { Name = "Orwell", Country = "UK" };
            ctx.Authors.Add(author);
            await ctx.SaveChangesAsync();

            var repo = new BookRepository(ctx);

            var book = new Book
            {
                Title = "1984",
                Year = 1949,
                Genre = "Dystopia",
                IsRead = false,
                AuthorId = author.Id
            };

            await repo.AddAsync(book);

            var fetched = await repo.GetByIdAsync(book.Id);

            Assert.NotNull(fetched);
            Assert.Equal("1984", fetched!.Title);
            Assert.NotNull(fetched.Author);
            Assert.Equal(author.Name, fetched.Author!.Name);
        }
        finally
        {
            await conn.CloseAsync();
            conn.Dispose();
        }
    }
}
