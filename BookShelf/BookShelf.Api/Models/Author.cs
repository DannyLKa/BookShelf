using System.Text.Json.Serialization;

namespace BookShelf.Api.Models;

public class Author
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Country { get; set; }

    [JsonIgnore]          // чтобы не было Book -> Author -> Books -> Author ...
    public List<Book>? Books { get; set; }
}
