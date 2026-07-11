using Library.Api.Application.Interfaces;
using Library.Api.Domain.Entities;
using Library.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Infrastructure.Repositories;

public class BookRepository : IBookRepository
{
    private readonly LibraryDbContext _context;

    public BookRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public Task<Book?> GetByIdAsync(Guid id) =>
        _context.Books.FirstOrDefaultAsync(b => b.Id == id);

    public Task<Book?> GetByIsbnAsync(string isbn) =>
        _context.Books.FirstOrDefaultAsync(b => b.Isbn == isbn);

    public Task<List<Book>> GetAllAsync() =>
        _context.Books.AsNoTracking().ToListAsync();

    public async Task AddAsync(Book book) =>
        await _context.Books.AddAsync(book);

    public void Update(Book book) =>
        _context.Books.Update(book);

    public void Delete(Book book) =>
        _context.Books.Remove(book);

    public async Task<bool> SaveChangesAsync() =>
        await _context.SaveChangesAsync() >= 0;
}
