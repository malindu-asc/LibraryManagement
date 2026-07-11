using Library.Api.Contracts.Members;

namespace Library.Api.Application.Interfaces;

public interface IMemberService
{
    Task<MemberResponse> CreateAsync(CreateMemberRequest request);
    Task<List<MemberResponse>> GetAllAsync();
    Task<MemberResponse> GetByIdAsync(Guid id);
    Task<MemberResponse> UpdateAsync(Guid id, UpdateMemberRequest request);
    Task DeleteAsync(Guid id);
}
