using LibraryApi.Services.Interfaces;
using LibraryApi.Repositories.Interfaces;

namespace LibraryApi.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _repository;
    public BookService(IBookRepository repository)
    {
        _repository = repository;
    }
}