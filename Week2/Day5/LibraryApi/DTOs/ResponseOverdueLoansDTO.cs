namespace LibraryApi.DTOs;

public class ResponseOverdueLoansDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Fullname { get; set; } = string.Empty;
    public int DaysOverdue { get; set; }
}