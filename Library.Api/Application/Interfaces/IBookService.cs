using Library.Api.Contracts.Books;

namespace Library.Api.Application.Interfaces;

public interface IBookService
{
    Task<BookResponse> CreateAsync(CreateBookRequest request);
    Task<List<BookResponse>> GetAllAsync();
    Task<BookResponse> GetByIdAsync(Guid id);
    Task<BookResponse> UpdateAsync(Guid id, UpdateBookRequest request);
    Task DeleteAsync(Guid id);
}
