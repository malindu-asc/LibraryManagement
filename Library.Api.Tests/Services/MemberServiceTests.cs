using Library.Api.Application.Interfaces;
using Library.Api.Application.Services;
using Library.Api.Contracts.Members;
using Library.Api.Domain.Entities;
using Library.Api.Domain.Exceptions;
using Moq;

namespace Library.Api.Tests.Services;

public class MemberServiceTests
{
    private readonly Mock<IMemberRepository> _memberRepository = new();
    private readonly MemberService _sut;

    public MemberServiceTests()
    {
        _sut = new MemberService(_memberRepository.Object);
    }

    [Fact]
    public async Task CreateAsync_WithDuplicateEmail_ThrowsConflictException()
    {
        var existingMember = new Member("Alice Johnson", "alice.johnson@example.com", "0771234567");
        _memberRepository.Setup(r => r.GetByEmailAsync("alice.johnson@example.com")).ReturnsAsync(existingMember);

        var request = new CreateMemberRequest
        {
            FullName = "Alice Clone",
            Email = "alice.johnson@example.com",
            PhoneNumber = "0777777777"
        };

        await Assert.ThrowsAsync<ConflictException>(() => _sut.CreateAsync(request));
    }
}
