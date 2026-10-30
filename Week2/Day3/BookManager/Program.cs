using Microsoft.Extensions.Hosting;
using Npgsql;
using BookManager.Data;
using BookManager.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = Host.CreateApplicationBuilder(args);

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
    var repository = scope.ServiceProvider.GetRequiredService<IBookRepository>();

    // var books = await repository.GetAllAsync();

    // foreach (var book in books)
    // {
    //     Console.WriteLine(book.Title);
    // }
    // Book book = new Book
    // {
    //     Title = "The Great Gatsby",
    //     Price = 1000,
    //     AuthorId = 1,
    //     PublishedYear = 1925,
    //     TotalCopies = 100,
    //     AvailableCopies = 100,
    //     Isbn = "9780679720218"
    // };
    // var id = await repository.CreateAsync(book);
    // Console.WriteLine($"Book generated with the Id of {id}");
    // bool isUpdated = await repository.UpdateAsync(12, new Book
    // {
    //     Title = "The Great",
    //     Price = 1000,
    //     AuthorId = 1,
    //     PublishedYear = 1925,
    //     TotalCopies = 100,
    //     AvailableCopies = 100,
    //     Isbn = "978-0743273565"
    // });
    // Console.WriteLine(isUpdated ? "The book has been updated." : "Some error occured");
    // bool isDeleted = await repository.DeleteAsync(12);
    // Console.WriteLine(isDeleted ? "The book has been Deleted." : "Some error occured");
    // var books = await repository.SearchAsync( 1, 2,title:"the");
    // Explicitly naming the required parameters
    var books = await repository.SearchAsync(2, 2, title: "the");

    foreach (var book in books)
    {
        Console.WriteLine(book.Title);
    }




    // var repository = scope.ServiceProvider.GetRequiredService<IAuthorRepository>();
    // var authors = await repository.GetAllAsync();
    // foreach (var author in authors)
    // {
    //     Console.WriteLine(author.Name);
    // }
    // Author author = new Author
    // {
    //     Name = "George Orwell",
    //     Country = "England"
    // };
    // int id = await repository.CreateAsync(author);
    // Console.WriteLine($"Author added with the id {id}");
    // Author author = new Author
    // {
    //     Name = "George",
    //     Country = "England"
    // };
    // bool isUpdated = await repository.UpdateAsync(4, author);
    // Console.WriteLine(isUpdated ? "The author has been updated." : "Some error occured");

    // bool isDeleted = await repository.DeleteAsync(4);
    // Console.WriteLine(isDeleted ? "The author has been Deleted." : "Some error occured");

    // var repository = scope.ServiceProvider.GetRequiredService<IMemberRepository>();
    // var members = await repository.GetAllAsync();
    // foreach (var member in members)
    // {
    //     Console.WriteLine(member.FullName);
    // }
    // Member member = new Member
    // {
    //     FullName = "Awais Ali",
    //     Email = "awais@mail.com",
    //     JoinedOn = DateOnly.FromDateTime(DateTime.Now)
    // };
    // int id = await repository.CreateAsync(member);
    // Console.WriteLine($"Member Inserted with Id {id}");
    // Member member = new Member
    // {
    //     FullName = "Awais Ali",
    //     Email = "awaisali@mail.com",
    //     JoinedOn = DateOnly.FromDateTime(DateTime.Now)
    // };
    // bool isUpdated = await repository.UpdateAsync(6, member);
    // Console.WriteLine(isUpdated ? "The member has been updated." : "Some error occured");
    // bool isDeleted = await repository.DeleteAsync(6);
    // Console.WriteLine(isDeleted ? "The member has been Deleted." : "Some error occured");
    // var repository = scope.ServiceProvider.GetRequiredService<ILoanRepository>();

    // await repository.BorrowAsync(1, 1, 2);
    // await repository.BorrowAsync(2, 2, 2);
    // await repository.BorrowAsync(3, 3, 2);
    // await repository.BorrowAsync(4, 4, 2);
    // await repository.BorrowAsync(5, 5, 2);


    // await repository.ReturnLoan(22);
    // await repository.ReturnLoan(23);
    // await repository.ReturnLoan(24);
    // await repository.ReturnLoan(25);
    // await repository.ReturnLoan(17);
}

await host.StopAsync();
