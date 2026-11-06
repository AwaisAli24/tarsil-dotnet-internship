namespace LibraryApi.DTOs;

public class ResponseAuthorReportDTO
{
    public string AuthorName { get; set; } = string.Empty;
    public int NumberOfBooks { get; set; }
    public int TotalCopies { get; set; }
    public int CurrentlyBorrowedCopies { get; set; }
}