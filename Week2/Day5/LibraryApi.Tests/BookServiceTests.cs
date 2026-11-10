using LibraryApi.DTOs;
using LibraryApi.Services;
using LibraryApi.Tests.Fakes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace LibraryApi.Tests;

public class BookServiceTests
{
    private readonly FakeBookRepository _fakeRepo;
    private readonly BookService _bookService;

    public BookServiceTests()
    {
        _fakeRepo = new FakeBookRepository();
        _bookService = new BookService(_fakeRepo, NullLogger<BookService>.Instance);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1.5)]
    public async Task CreateAsync_WhenPriceIsZeroOrNegative_ThrowsBadHttpRequestExceptionForInvalidPrice(decimal invalidPrice)
    {
        // Arrange
        var bookDto = new CreateBookDTO
        {
            Title = "Test Book",
            Isbn = "9781234567890",
            Price = invalidPrice,
            TotalCopies = 5,
            AvailableCopies = 5,
            PublishedYear = 2024,
            AuthorId = 1
        };

        // Act & Assert
        var ex = await Assert.ThrowsAsync<BadHttpRequestException>(
            () => _bookService.CreateAsync(bookDto));

        Assert.Equal("Price should be greater than 0.", ex.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public async Task UpdateAsync_WhenPriceIsZeroOrNegative_ThrowsBadHttpRequestExceptionForInvalidPrice(decimal invalidPrice)
    {
        // Arrange
        const int bookId = 1;
        _fakeRepo.AddExistingBook(new ResponseBookDTO
        {
            Id = bookId,
            Title = "Existing Book",
            Isbn = "9780000000001",
            Price = 20.0m,
            TotalCopies = 5,
            AvailableCopies = 5,
            PublishedYear = 2020,
            AuthorId = 1
        });

        var updateDto = new UpdateBookDTO
        {
            Title = "Updated Title",
            Isbn = "9780000000001",
            Price = invalidPrice,
            TotalCopies = 5,
            AvailableCopies = 5,
            PublishedYear = 2020,
            AuthorId = 1
        };

        // Act & Assert
        var ex = await Assert.ThrowsAsync<BadHttpRequestException>(
            () => _bookService.UpdateAsync(bookId, updateDto));

        Assert.Equal("Price should be greater than 0.", ex.Message);
    }

    [Fact]
    public async Task CreateAsync_WhenIsbnAlreadyExists_ThrowsArgumentExceptionForDuplicateIsbn()
    {
        // Arrange
        const string duplicateIsbn = "978-0-123456-47-2";
        _fakeRepo.AddExistingBook(new ResponseBookDTO
        {
            Id = 1,
            Title = "Original Book",
            Isbn = duplicateIsbn,
            Price = 15.0m,
            TotalCopies = 2,
            AvailableCopies = 2,
            PublishedYear = 2021,
            AuthorId = 1
        });

        var newBookDto = new CreateBookDTO
        {
            Title = "Another Book With Same ISBN",
            Isbn = duplicateIsbn,
            Price = 19.99m,
            TotalCopies = 3,
            AvailableCopies = 3,
            PublishedYear = 2023,
            AuthorId = 1
        };

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(
            () => _bookService.CreateAsync(newBookDto));

        Assert.Equal("ISBN already Exist", ex.Message);
    }

    [Fact]
    public async Task UpdateAsync_WhenNewIsbnAlreadyUsedByAnotherBook_ThrowsArgumentExceptionForDuplicateIsbn()
    {
        // Arrange
        const string takenIsbn = "978-1-111111-11-1";

        _fakeRepo.AddExistingBook(new ResponseBookDTO
        {
            Id = 1,
            Title = "Book One",
            Isbn = takenIsbn,
            Price = 10.0m,
            TotalCopies = 2,
            AvailableCopies = 2,
            PublishedYear = 2019,
            AuthorId = 1
        });

        _fakeRepo.AddExistingBook(new ResponseBookDTO
        {
            Id = 2,
            Title = "Book Two",
            Isbn = "978-2-222222-22-2",
            Price = 12.0m,
            TotalCopies = 3,
            AvailableCopies = 3,
            PublishedYear = 2020,
            AuthorId = 1
        });

        var updateDto = new UpdateBookDTO
        {
            Title = "Book Two With Conflicting ISBN",
            Isbn = takenIsbn, // Collides with Book One
            Price = 12.0m,
            TotalCopies = 3,
            AvailableCopies = 3,
            PublishedYear = 2020,
            AuthorId = 1
        };

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(
            () => _bookService.UpdateAsync(2, updateDto));

        Assert.Equal("ISBN already Exist", ex.Message);
    }

    [Fact]
    public async Task GetByIdAsync_WhenBookDoesNotExist_ReturnsNullForBookNotFound()
    {
        // Arrange
        const int nonExistentId = 999;

        // Act
        var result = await _bookService.GetByIdAsync(nonExistentId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_WhenBookDoesNotExist_ReturnsNullForBookNotFound()
    {
        // Arrange
        const int nonExistentId = 888;
        var updateDto = new UpdateBookDTO
        {
            Title = "Non-existent Book",
            Isbn = "9789999999999",
            Price = 15.0m,
            TotalCopies = 2,
            AvailableCopies = 2,
            PublishedYear = 2022,
            AuthorId = 1
        };

        // Act
        var result = await _bookService.UpdateAsync(nonExistentId, updateDto);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_WhenValidUpdateData_UpdatesBookAndReturnsUpdatedResponse()
    {
        // Arrange
        const int bookId = 10;
        _fakeRepo.AddExistingBook(new ResponseBookDTO
        {
            Id = bookId,
            Title = "Original Title",
            Isbn = "9781111222233",
            Price = 15.0m,
            TotalCopies = 4,
            AvailableCopies = 4,
            PublishedYear = 2018,
            AuthorId = 2
        });

        var updateDto = new UpdateBookDTO
        {
            Title = "Brand New Title",
            Isbn = "9781111222233",
            Price = 29.99m,
            TotalCopies = 6,
            AvailableCopies = 5,
            PublishedYear = 2025,
            AuthorId = 2
        };

        // Act
        var updated = await _bookService.UpdateAsync(bookId, updateDto);

        // Assert
        Assert.NotNull(updated);
        Assert.Equal(bookId, updated.Id);
        Assert.Equal("Brand New Title", updated.Title);
        Assert.Equal(29.99m, updated.Price);
        Assert.Equal(6, updated.TotalCopies);
        Assert.Equal(5, updated.AvailableCopies);
        Assert.Equal(2025, updated.PublishedYear);

        // Check fake repo was updated in-memory
        var bookInRepo = await _fakeRepo.GetByIdAsync(bookId);
        Assert.NotNull(bookInRepo);
        Assert.Equal("Brand New Title", bookInRepo.Title);
        Assert.Equal(29.99m, bookInRepo.Price);
    }

    [Fact]
    public async Task CreateAsync_WhenValidData_SuccessfullyCreatesBook()
    {
        // Arrange
        var createDto = new CreateBookDTO
        {
            Title = "Clean Architecture",
            Isbn = "9780134494166",
            Price = 34.50m,
            TotalCopies = 10,
            AvailableCopies = 10,
            PublishedYear = 2017,
            AuthorId = 1
        };

        // Act
        var created = await _bookService.CreateAsync(createDto);

        // Assert
        Assert.NotNull(created);
        Assert.True(created.Id > 0);
        Assert.Equal("Clean Architecture", created.Title);
        Assert.Equal("9780134494166", created.Isbn);
        Assert.Equal(34.50m, created.Price);
        Assert.Equal(10, created.TotalCopies);
        Assert.Equal(10, created.AvailableCopies);
    }

    [Fact]
    public async Task CreateAsync_WhenAvailableCopiesGreaterThanTotalCopies_ThrowsBadHttpRequestException()
    {
        // Arrange
        var createDto = new CreateBookDTO
        {
            Title = "Invalid Copies Book",
            Isbn = "9780000000002",
            Price = 20.0m,
            TotalCopies = 3,
            AvailableCopies = 5, // Invalid: > TotalCopies
            PublishedYear = 2023,
            AuthorId = 1
        };

        // Act & Assert
        var ex = await Assert.ThrowsAsync<BadHttpRequestException>(
            () => _bookService.CreateAsync(createDto));

        Assert.Equal("Available copies should be less or equal than the total copies.", ex.Message);
    }
}

