using Library.Api.Application.Interfaces;
using Library.Api.Contracts.Members;
using Library.Api.Domain.Entities;
using Library.Api.Domain.Exceptions;

namespace Library.Api.Application.Services;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _memberRepository;

    public MemberService(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<MemberResponse> CreateAsync(CreateMemberRequest request)
    {
        var existing = await _memberRepository.GetByEmailAsync(request.Email);
        if (existing is not null)
            throw new ConflictException("A member with this email already exists.");

        var member = new Member(request.FullName, request.Email, request.PhoneNumber);

        await _memberRepository.AddAsync(member);
        await _memberRepository.SaveChangesAsync();

        return ToResponse(member);
    }

    public async Task<List<MemberResponse>> GetAllAsync()
    {
        var members = await _memberRepository.GetAllAsync();
        return members.Select(ToResponse).ToList();
    }

    public async Task<MemberResponse> GetByIdAsync(Guid id)
    {
        var member = await _memberRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Member not found.");

        return ToResponse(member);
    }

    public async Task<MemberResponse> UpdateAsync(Guid id, UpdateMemberRequest request)
    {
        var member = await _memberRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Member not found.");

        var existing = await _memberRepository.GetByEmailAsync(request.Email);
        if (existing is not null && existing.Id != id)
            throw new ConflictException("A member with this email already exists.");

        member.Update(request.FullName, request.Email, request.PhoneNumber, request.IsActive);

        _memberRepository.Update(member);
        await _memberRepository.SaveChangesAsync();

        return ToResponse(member);
    }

    public async Task DeleteAsync(Guid id)
    {
        var member = await _memberRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Member not found.");

        _memberRepository.Delete(member);
        await _memberRepository.SaveChangesAsync();
    }

    private static MemberResponse ToResponse(Member member) => new()
    {
        Id = member.Id,
        FullName = member.FullName,
        Email = member.Email,
        PhoneNumber = member.PhoneNumber,
        RegisteredDate = member.RegisteredDate,
        IsActive = member.IsActive
    };
}
