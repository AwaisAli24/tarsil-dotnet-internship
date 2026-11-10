using LibraryApi.DTOs;
using LibraryApi.Repositories.Interfaces;

namespace LibraryApi.Tests.Fakes;

public class FakeLoanRepository : ILoanRepository
{
    public class BookItem
    {
        public int Id { get; set; }
        public int AvailableCopies { get; set; }
    }

    public class LoanItem
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int MemberId { get; set; }
        public DateTime BorrowedAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
    }

    public List<BookItem> Books { get; } = new();
    public HashSet<int> MemberIds { get; } = new();
    public List<LoanItem> Loans { get; } = new();
    private int _nextLoanId = 1;

    public void AddBook(int id, int availableCopies)
    {
        Books.Add(new BookItem { Id = id, AvailableCopies = availableCopies });
    }

    public void AddMember(int id)
    {
        MemberIds.Add(id);
    }

    public LoanItem AddLoan(int id, int bookId, int memberId, DateTime? returnedAt = null)
    {
        var loan = new LoanItem
        {
            Id = id,
            BookId = bookId,
            MemberId = memberId,
            BorrowedAt = DateTime.UtcNow,
            ReturnedAt = returnedAt
        };
        Loans.Add(loan);
        if (id >= _nextLoanId)
        {
            _nextLoanId = id + 1;
        }
        return loan;
    }

    public Task<int> BorrowAsync(CreateLoanDTO loan)
    {
        var memberExists = MemberIds.Contains(loan.MemberId);
        var book = Books.FirstOrDefault(b => b.Id == loan.BookId);

        if (!memberExists || book == null)
        {
            throw new ArgumentException("Book or member does Not exist.");
        }

        var activeLoansCount = Loans.Count(l => l.MemberId == loan.MemberId && l.ReturnedAt == null);
        if (activeLoansCount >= 3)
        {
            throw new InvalidOperationException("Member already has 3 loans, cannot take more loans.");
        }

        if (book.AvailableCopies <= 0)
        {
            throw new InvalidOperationException("Book is not available.");
        }

        book.AvailableCopies--;
        var loanId = _nextLoanId++;
        Loans.Add(new LoanItem
        {
            Id = loanId,
            BookId = loan.BookId,
            MemberId = loan.MemberId,
            BorrowedAt = DateTime.UtcNow,
            ReturnedAt = null
        });

        return Task.FromResult(loanId);
    }

    public Task<int> ReturnAsync(int id)
    {
        var loan = Loans.FirstOrDefault(l => l.Id == id);
        if (loan == null)
        {
            throw new KeyNotFoundException("Provided id does not exist in loans.");
        }

        if (loan.ReturnedAt != null)
        {
            throw new InvalidOperationException("The Book is Already returned.");
        }

        loan.ReturnedAt = DateTime.UtcNow;
        var book = Books.FirstOrDefault(b => b.Id == loan.BookId);
        if (book != null)
        {
            book.AvailableCopies++;
        }

        return Task.FromResult(id);
    }
}

