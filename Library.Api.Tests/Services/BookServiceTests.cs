using Library.Api.Application.Interfaces;
using Library.Api.Application.Services;
using Library.Api.Contracts.Books;
using Library.Api.Domain.Entities;
using Library.Api.Domain.Exceptions;
using Moq;

namespace Library.Api.Tests.Services;

public class BookServiceTests
{
    private readonly Mock<IBookRepository> _bookRepository = new();
    private readonly BookService _sut;

    public BookServiceTests()
    {
        _sut = new BookService(_bookRepository.Object);
    }

    [Fact]
    public async Task CreateAsync_WithDuplicateIsbn_ThrowsConflictException()
    {
        var existingBook = new Book("Clean Code", "Robert C. Martin", "9780132350884", 2008, 3);
        _bookRepository.Setup(r => r.GetByIsbnAsync("9780132350884")).ReturnsAsync(existingBook);

        var request = new CreateBookRequest
        {
            Title = "Clean Code (Copy)",
            Author = "Robert C. Martin",
            Isbn = "9780132350884",
            PublishedYear = 2008,
            TotalCopies = 1
        };

        await Assert.ThrowsAsync<ConflictException>(() => _sut.CreateAsync(request));
    }
}
