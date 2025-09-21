using BookShelf.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShelf.Api.Data;

public class LibraryContext : DbContext
{
    public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

    public DbSet<Book> Books => Set<Book>();
    public DbSet<Author> Authors => Set<Author>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Таблицы в нижнем регистре — дружелюбно к PostgreSQL
        modelBuilder.Entity<Book>().ToTable("books");
        modelBuilder.Entity<Author>().ToTable("authors");

        // Связь и FK
        modelBuilder.Entity<Book>()
            .HasOne(b => b.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(b => b.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        // Базовая валидация
        modelBuilder.Entity<Book>().Property(b => b.Title).IsRequired();
    }
}
