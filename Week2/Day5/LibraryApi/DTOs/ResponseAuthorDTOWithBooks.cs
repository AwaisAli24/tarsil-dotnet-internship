namespace LibraryApi.DTOs;

public class ResponseAuthorDTOWithBooks
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public List<ResponseBookDTO> Books { get; set; } = new List<ResponseBookDTO>();
}