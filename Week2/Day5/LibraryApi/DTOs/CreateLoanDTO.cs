namespace LibraryApi.DTOs;

public class CreateLoanDTO
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public int MemberId { get; set; }
    public int days { get; set; }
    public DateTime BorrowedAt { get; set; } = DateTime.Now;
}