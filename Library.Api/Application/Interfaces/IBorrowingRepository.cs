using Library.Api.Domain.Entities;

namespace Library.Api.Application.Interfaces;

public interface IBorrowingRepository
{
    Task<Borrowing?> GetByIdAsync(Guid id);
    Task<List<Borrowing>> GetAllAsync();
    Task<List<Borrowing>> GetByMemberIdAsync(Guid memberId);
    Task<int> GetActiveBorrowCountForMemberAsync(Guid memberId);
    Task AddAsync(Borrowing borrowing);
    void Update(Borrowing borrowing);
    Task<bool> SaveChangesAsync();
}
