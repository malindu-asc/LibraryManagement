using Library.Api.Application.Interfaces;
using Library.Api.Domain.Entities;
using Library.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Infrastructure.Repositories;

public class MemberRepository : IMemberRepository
{
    private readonly LibraryDbContext _context;

    public MemberRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public Task<Member?> GetByIdAsync(Guid id) =>
        _context.Members.FirstOrDefaultAsync(m => m.Id == id);

    public Task<Member?> GetByEmailAsync(string email) =>
        _context.Members.FirstOrDefaultAsync(m => m.Email == email);

    public Task<List<Member>> GetAllAsync() =>
        _context.Members.AsNoTracking().ToListAsync();

    public async Task AddAsync(Member member) =>
        await _context.Members.AddAsync(member);

    public void Update(Member member) =>
        _context.Members.Update(member);

    public void Delete(Member member) =>
        _context.Members.Remove(member);

    public async Task<bool> SaveChangesAsync() =>
        await _context.SaveChangesAsync() >= 0;
}
