using Library.Api.Application.Interfaces;
using Library.Api.Contracts.Books;
using Library.Api.Domain.Entities;
using Library.Api.Domain.Exceptions;

namespace Library.Api.Application.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<BookResponse> CreateAsync(CreateBookRequest request)
    {
        if (request.PublishedYear > DateTime.UtcNow.Year)
            throw new BusinessRuleException("PublishedYear cannot be in the future.");

        var existing = await _bookRepository.GetByIsbnAsync(request.Isbn);
        if (existing is not null)
            throw new ConflictException("A book with this ISBN already exists.");

        var book = new Book(request.Title, request.Author, request.Isbn, request.PublishedYear, request.TotalCopies);

        await _bookRepository.AddAsync(book);
        await _bookRepository.SaveChangesAsync();

        return ToResponse(book);
    }

    public async Task<List<BookResponse>> GetAllAsync()
    {
        var books = await _bookRepository.GetAllAsync();
        return books.Select(ToResponse).ToList();
    }

    public async Task<BookResponse> GetByIdAsync(Guid id)
    {
        var book = await _bookRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Book not found.");

        return ToResponse(book);
    }

    public async Task<BookResponse> UpdateAsync(Guid id, UpdateBookRequest request)
    {
        var book = await _bookRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Book not found.");

        if (request.PublishedYear > DateTime.UtcNow.Year)
            throw new BusinessRuleException("PublishedYear cannot be in the future.");

        var existing = await _bookRepository.GetByIsbnAsync(request.Isbn);
        if (existing is not null && existing.Id != id)
            throw new ConflictException("A book with this ISBN already exists.");

        book.Update(request.Title, request.Author, request.Isbn, request.PublishedYear, request.TotalCopies);

        _bookRepository.Update(book);
        await _bookRepository.SaveChangesAsync();

        return ToResponse(book);
    }

    public async Task DeleteAsync(Guid id)
    {
        var book = await _bookRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Book not found.");

        _bookRepository.Delete(book);
        await _bookRepository.SaveChangesAsync();
    }

    private static BookResponse ToResponse(Book book) => new()
    {
        Id = book.Id,
        Title = book.Title,
        Author = book.Author,
        Isbn = book.Isbn,
        PublishedYear = book.PublishedYear,
        TotalCopies = book.TotalCopies,
        AvailableCopies = book.AvailableCopies
    };
}
