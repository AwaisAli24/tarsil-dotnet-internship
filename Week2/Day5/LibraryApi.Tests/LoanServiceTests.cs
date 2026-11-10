using LibraryApi.DTOs;
using LibraryApi.Services;
using LibraryApi.Tests.Fakes;
using Xunit;

namespace LibraryApi.Tests;

public class LoanServiceTests
{
    private readonly FakeLoanRepository _fakeRepo;
    private readonly LoanService _loanService;

    public LoanServiceTests()
    {
        _fakeRepo = new FakeLoanRepository();
        _loanService = new LoanService(_fakeRepo);
    }

    [Fact]
    public async Task BorrowAsync_WhenValidRequestAndCopiesAvailable_BorrowSucceeds()
    {
        // Arrange
        const int bookId = 1;
        const int memberId = 100;
        _fakeRepo.AddBook(bookId, availableCopies: 2);
        _fakeRepo.AddMember(memberId);

        var loanDto = new CreateLoanDTO
        {
            BookId = bookId,
            MemberId = memberId,
            days = 14
        };

        // Act
        var loanId = await _loanService.BorrowAsync(loanDto);

        // Assert
        Assert.True(loanId > 0);
        var book = Assert.Single(_fakeRepo.Books, b => b.Id == bookId);
        Assert.Equal(1, book.AvailableCopies);

        var recordedLoan = Assert.Single(_fakeRepo.Loans, l => l.Id == loanId);
        Assert.Equal(bookId, recordedLoan.BookId);
        Assert.Equal(memberId, recordedLoan.MemberId);
        Assert.Null(recordedLoan.ReturnedAt);
    }

    [Fact]
    public async Task BorrowAsync_WhenNoCopiesLeft_ThrowsInvalidOperationException()
    {
        // Arrange
        const int bookId = 2;
        const int memberId = 101;
        _fakeRepo.AddBook(bookId, availableCopies: 0);
        _fakeRepo.AddMember(memberId);

        var loanDto = new CreateLoanDTO
        {
            BookId = bookId,
            MemberId = memberId,
            days = 7
        };

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _loanService.BorrowAsync(loanDto));

        Assert.Equal("Book is not available.", ex.Message);
    }

    [Fact]
    public async Task BorrowAsync_WhenMemberHasThreeActiveLoans_ThrowsInvalidOperationExceptionForThreeLoanLimit()
    {
        // Arrange
        const int bookId = 3;
        const int memberId = 102;
        _fakeRepo.AddBook(bookId, availableCopies: 5);
        _fakeRepo.AddMember(memberId);

        // Member already has 3 unreturned loans
        _fakeRepo.AddLoan(id: 1, bookId: bookId, memberId: memberId, returnedAt: null);
        _fakeRepo.AddLoan(id: 2, bookId: bookId, memberId: memberId, returnedAt: null);
        _fakeRepo.AddLoan(id: 3, bookId: bookId, memberId: memberId, returnedAt: null);

        var loanDto = new CreateLoanDTO
        {
            BookId = bookId,
            MemberId = memberId,
            days = 7
        };

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _loanService.BorrowAsync(loanDto));

        Assert.Equal("Member already has 3 loans, cannot take more loans.", ex.Message);
    }

    [Fact]
    public async Task ReturnAsync_WhenReturningAlreadyReturnedLoan_ThrowsInvalidOperationExceptionForReturnTwice()
    {
        // Arrange
        const int bookId = 4;
        const int memberId = 103;
        const int loanId = 50;

        _fakeRepo.AddBook(bookId, availableCopies: 1);
        _fakeRepo.AddMember(memberId);
        _fakeRepo.AddLoan(loanId, bookId, memberId, returnedAt: null);

        // First return succeeds
        var firstReturnResult = await _loanService.ReturnAsync(loanId);
        Assert.Equal(loanId, firstReturnResult);

        // Act & Assert: second return attempt on the same loan
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _loanService.ReturnAsync(loanId));

        Assert.Equal("The Book is Already returned.", ex.Message);
    }

    [Fact]
    public async Task ReturnAsync_WhenLoanExistsAndActive_ReturnsSuccessfullyAndRestoresCopy()
    {
        // Arrange
        const int bookId = 5;
        const int memberId = 104;
        const int loanId = 60;

        _fakeRepo.AddBook(bookId, availableCopies: 0);
        _fakeRepo.AddMember(memberId);
        _fakeRepo.AddLoan(loanId, bookId, memberId, returnedAt: null);

        // Act
        var result = await _loanService.ReturnAsync(loanId);

        // Assert
        Assert.Equal(loanId, result);
        var book = Assert.Single(_fakeRepo.Books, b => b.Id == bookId);
        Assert.Equal(1, book.AvailableCopies);

        var loan = Assert.Single(_fakeRepo.Loans, l => l.Id == loanId);
        Assert.NotNull(loan.ReturnedAt);
    }
}

