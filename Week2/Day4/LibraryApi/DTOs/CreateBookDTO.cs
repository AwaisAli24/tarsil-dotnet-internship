using System.ComponentModel.DataAnnotations;

public class CreateBookDTO
{
    [Required]
    public string Title { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    [Range(0.01, int.MaxValue)]
    public decimal Price { get; set; }
    [Range(1, int.MaxValue)]
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }
    [Range(1, int.MaxValue)]
    public int PublishedYear { get; set; }
    public int AuthorId { get; set; }
}