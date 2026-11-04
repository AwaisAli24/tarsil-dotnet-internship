using Microsoft.Extensions.Hosting;
using Npgsql;
using BookManager.Data;
using BookManager.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddUserSecrets<Program>();

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (connectionString == null)
{
    Console.WriteLine("ConnectionString is not set.");
    return;
}
builder.Services.AddSingleton(NpgsqlDataSource.Create(connectionString));

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();


using var host = builder.Build();

await host.StartAsync();

using (var scope = host.Services.CreateScope())
{
    // ── Book Repository ──────────────────────────────────────────────────
    var bookRepository = scope.ServiceProvider.GetRequiredService<IBookRepository>();

    var allBooks = await bookRepository.GetAllAsync();
    foreach (var b in allBooks)
    {
        Console.WriteLine(b.Title);
    }

    var foundBook = await bookRepository.GetByIdAsync(999);
    Console.WriteLine(foundBook?.Title ?? "Book not found");

    var newBook = new Book
    {
        Title = "The Great Gatsby",
        Price = 1000,
        AuthorId = 100,
        PublishedYear = 1925,
        TotalCopies = 100,
        AvailableCopies = 100,
        Isbn = "978067977620218"
    };
    var newBookId = await bookRepository.CreateAsync(newBook);
    Console.WriteLine($"Book generated with the Id of {newBookId}");

    bool isBookUpdated = await bookRepository.UpdateAsync(12, new Book
    {
        Title = "The Great",
        Price = 1000,
        AuthorId = 1,
        PublishedYear = 1925,
        TotalCopies = 100,
        AvailableCopies = 100,
        Isbn = "978-0743273565"
    });
    Console.WriteLine(isBookUpdated ? "The book has been updated." : "Some error occured");

    bool isBookDeleted = await bookRepository.DeleteAsync(12);
    Console.WriteLine(isBookDeleted ? "The book has been Deleted." : "Some error occured");

    var booksPage1 = await bookRepository.SearchAsync(1, 2, title: "the");
    // Explicitly naming the required parameters
    var booksPage2 = await bookRepository.SearchAsync(2, 2, title: "the");

    foreach (var b in booksPage2)
    {
        Console.WriteLine(b.Title);
    }

    // ── Author Repository ────────────────────────────────────────────────
    var authorRepository = scope.ServiceProvider.GetRequiredService<IAuthorRepository>();
    var authors = await authorRepository.GetAllAsync();
    foreach (var a in authors)
    {
        Console.WriteLine(a.Name);
    }

    var newAuthor = new Author
    {
        Name = "George Orwell",
        Country = "England"
    };
    int newAuthorId = await authorRepository.CreateAsync(newAuthor);
    Console.WriteLine($"Author added with the id {newAuthorId}");

    var updatedAuthor = new Author
    {
        Name = "George",
        Country = "England"
    };
    bool isAuthorUpdated = await authorRepository.UpdateAsync(4, updatedAuthor);
    Console.WriteLine(isAuthorUpdated ? "The author has been updated." : "Some error occured");

    bool isAuthorDeleted = await authorRepository.DeleteAsync(4);
    Console.WriteLine(isAuthorDeleted ? "The author has been Deleted." : "Some error occured");

    // ── Member Repository ────────────────────────────────────────────────
    var memberRepository = scope.ServiceProvider.GetRequiredService<IMemberRepository>();
    var members = await memberRepository.GetAllAsync();
    foreach (var m in members)
    {
        Console.WriteLine(m.FullName);
    }

    var newMember = new Member
    {
        FullName = "Awais Ali",
        Email = "awais@mail.com",
        JoinedOn = DateOnly.FromDateTime(DateTime.Now)
    };
    int newMemberId = await memberRepository.CreateAsync(newMember);
    Console.WriteLine($"Member Inserted with Id {newMemberId}");

    var updatedMember = new Member
    {
        FullName = "Awais Ali",
        Email = "awaisali@mail.com",
        JoinedOn = DateOnly.FromDateTime(DateTime.Now)
    };
    bool isMemberUpdated = await memberRepository.UpdateAsync(6, updatedMember);
    Console.WriteLine(isMemberUpdated ? "The member has been updated." : "Some error occured");

    bool isMemberDeleted = await memberRepository.DeleteAsync(6);
    Console.WriteLine(isMemberDeleted ? "The member has been Deleted." : "Some error occured");

    // ── Loan Repository ──────────────────────────────────────────────────
    var loanRepository = scope.ServiceProvider.GetRequiredService<ILoanRepository>();

    await loanRepository.BorrowAsync(1, 1, 2);
    await loanRepository.BorrowAsync(2, 2, 2);
    await loanRepository.BorrowAsync(3, 3, 2);
    await loanRepository.BorrowAsync(4, 4, 2);
    await loanRepository.BorrowAsync(1, 5, 2);
    await loanRepository.BorrowAsync(1, 5, 2);
    await loanRepository.BorrowAsync(1, 5, 2);
    await loanRepository.BorrowAsync(1, 5, 2);
    await loanRepository.BorrowAsync(1, 5, 2);

    await loanRepository.ReturnLoan(22);
    await loanRepository.ReturnLoan(23);
    await loanRepository.ReturnLoan(24);
    await loanRepository.ReturnLoan(25);
    await loanRepository.ReturnLoan(999);
}

await host.StopAsync();
