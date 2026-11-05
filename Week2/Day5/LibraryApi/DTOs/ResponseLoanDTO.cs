namespace LibraryApi.DTOs;

public class ResponseLoanDTO
{
    public int Id { get; set; }
    public int BookTitle { get; set; }
    public int MemberId { get; set; }
    public bool IsOverdue { get; set; }
}