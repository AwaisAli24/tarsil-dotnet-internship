using System.ComponentModel.DataAnnotations;

public class UpdateAuthorDTO
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}