namespace BookManager.Data;

public interface ILoanRepository
{
    Task BorrowAsync(int bookId, int memberId, int date);
    Task ReturnLoan(int loanId);
}