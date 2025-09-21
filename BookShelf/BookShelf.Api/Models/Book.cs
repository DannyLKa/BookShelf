namespace BookShelf.Api.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public int Year { get; set; }
    public string? Genre { get; set; }
    public bool IsRead { get; set; }

    public int AuthorId { get; set; }
    public Author? Author { get; set; }
}
