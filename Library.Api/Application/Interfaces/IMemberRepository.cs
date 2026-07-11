using Library.Api.Domain.Entities;

namespace Library.Api.Application.Interfaces;

public interface IMemberRepository
{
    Task<Member?> GetByIdAsync(Guid id);
    Task<Member?> GetByEmailAsync(string email);
    Task<List<Member>> GetAllAsync();
    Task AddAsync(Member member);
    void Update(Member member);
    void Delete(Member member);
    Task<bool> SaveChangesAsync();
}
