using Library.Api.Application.Interfaces;
using Library.Api.Domain.Entities;
using Library.Api.Domain.Enums;
using Library.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Infrastructure.Repositories;

public class BorrowingRepository : IBorrowingRepository
{
    private readonly LibraryDbContext _context;

    public BorrowingRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public Task<Borrowing?> GetByIdAsync(Guid id) =>
        _context.Borrowings
            .Include(b => b.Book)
            .Include(b => b.Member)
            .FirstOrDefaultAsync(b => b.Id == id);

    public Task<List<Borrowing>> GetAllAsync() =>
        _context.Borrowings
            .Include(b => b.Book)
            .Include(b => b.Member)
            .AsNoTracking()
            .OrderByDescending(b => b.BorrowedDate)
            .ToListAsync();

    public Task<List<Borrowing>> GetByMemberIdAsync(Guid memberId) =>
        _context.Borrowings
            .Include(b => b.Book)
            .Include(b => b.Member)
            .AsNoTracking()
            .Where(b => b.MemberId == memberId)
            .OrderByDescending(b => b.BorrowedDate)
            .ToListAsync();

    public Task<int> GetActiveBorrowCountForMemberAsync(Guid memberId) =>
        _context.Borrowings.CountAsync(b =>
            b.MemberId == memberId &&
            (b.Status == BorrowingStatus.Borrowed || b.Status == BorrowingStatus.Overdue));

    public async Task AddAsync(Borrowing borrowing) =>
        await _context.Borrowings.AddAsync(borrowing);

    public void Update(Borrowing borrowing) =>
        _context.Borrowings.Update(borrowing);

    public async Task<bool> SaveChangesAsync() =>
        await _context.SaveChangesAsync() >= 0;
}
