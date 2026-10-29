namespace BookManager.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }
    public int PublishedYear { get; set; }
}