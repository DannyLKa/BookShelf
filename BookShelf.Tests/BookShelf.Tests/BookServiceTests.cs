using BookShelf.Api.Models;
using BookShelf.Api.Services;
using Xunit;

namespace BookShelf.Tests;

public class BookServiceTests
{
    [Fact]
    public async Task GetStatisticsAsync_counts_read_and_unread_correctly()
    {
        var (conn, ctx) = TestSqliteFactory.CreateContext();
        try
        {
            var author = new Author { Name = "Tester", Country = "UK" };
            ctx.Authors.Add(author);
            await ctx.SaveChangesAsync();

            ctx.Books.AddRange(
                new Book { Title = "Book A", Year = 2000, Genre = "Drama", IsRead = true, AuthorId = author.Id },
                new Book { Title = "Book B", Year = 2001, Genre = "Sci-Fi", IsRead = false, AuthorId = author.Id },
                new Book { Title = "Book C", Year = 2002, Genre = "Fantasy", IsRead = true, AuthorId = author.Id }
            );
            await ctx.SaveChangesAsync();

            var svc = new BookService(ctx);

            var stats = await svc.GetStatisticsAsync();

            Assert.Equal(2, stats.Read);
            Assert.Equal(1, stats.Unread);
        }
        finally
        {
            await conn.CloseAsync();
            conn.Dispose();
        }
    }
}
