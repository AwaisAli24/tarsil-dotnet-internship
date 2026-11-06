namespace LibraryApi.DTOs;

public class ResponseLoanDTO
{
    public int Id { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public int MemberId { get; set; }
    public bool IsActive { get; set; }
}