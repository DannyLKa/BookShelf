using BookShelf.Api.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BookShelf.Tests;

public static class TestSqliteFactory
{
    public static (SqliteConnection conn, LibraryContext ctx) CreateContext()
    {
        var conn = new SqliteConnection("Data Source=:memory:");
        conn.Open(); //  пока открыто — БД жива

        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseSqlite(conn)
            .Options;

        var ctx = new LibraryContext(options);
        ctx.Database.EnsureCreated(); // поднимаем схему по модели

        return (conn, ctx);
    }
}
