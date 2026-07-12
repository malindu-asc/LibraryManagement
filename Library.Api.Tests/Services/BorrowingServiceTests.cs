using Library.Api.Application.Interfaces;
using Library.Api.Application.Services;
using Library.Api.Contracts.Borrowings;
using Library.Api.Domain.Entities;
using Library.Api.Domain.Exceptions;
using Moq;

namespace Library.Api.Tests.Services;

public class BorrowingServiceTests
{
    private readonly Mock<IBorrowingRepository> _borrowingRepository = new();
    private readonly Mock<IBookRepository> _bookRepository = new();
    private readonly Mock<IMemberRepository> _memberRepository = new();
    private readonly BorrowingService _sut;

    public BorrowingServiceTests()
    {
        _sut = new BorrowingService(_borrowingRepository.Object, _bookRepository.Object, _memberRepository.Object);
    }

    [Fact]
    public async Task BorrowAsync_WhenNoAvailableCopies_ThrowsBusinessRuleException()
    {
        var book = new Book("Refactoring", "Martin Fowler", "9780134757599", 2018, 1);
        book.BorrowCopy();
        var member = new Member("Alice Johnson", "alice.johnson@example.com", null);

        _bookRepository.Setup(r => r.GetByIdAsync(book.Id)).ReturnsAsync(book);
        _memberRepository.Setup(r => r.GetByIdAsync(member.Id)).ReturnsAsync(member);
        _borrowingRepository.Setup(r => r.GetActiveBorrowCountForMemberAsync(member.Id)).ReturnsAsync(0);

        var request = new BorrowRequest { BookId = book.Id, MemberId = member.Id };

        await Assert.ThrowsAsync<BusinessRuleException>(() => _sut.BorrowAsync(request));
    }

    [Fact]
    public async Task BorrowAsync_WhenMemberIsInactive_ThrowsBusinessRuleException()
    {
        var book = new Book("Clean Code", "Robert C. Martin", "9780132350884", 2008, 3);
        var member = new Member("Bob Smith", "bob.smith@example.com", null);
        member.Update(member.FullName, member.Email, member.PhoneNumber, isActive: false);

        _bookRepository.Setup(r => r.GetByIdAsync(book.Id)).ReturnsAsync(book);
        _memberRepository.Setup(r => r.GetByIdAsync(member.Id)).ReturnsAsync(member);

        var request = new BorrowRequest { BookId = book.Id, MemberId = member.Id };

        await Assert.ThrowsAsync<BusinessRuleException>(() => _sut.BorrowAsync(request));
    }

    [Fact]
    public async Task BorrowAsync_WhenMemberHasThreeActiveBorrowings_ThrowsBusinessRuleException()
    {
        var book = new Book("Design Patterns", "Erich Gamma", "9780201633610", 1994, 4);
        var member = new Member("Carol White", "carol.white@example.com", null);

        _bookRepository.Setup(r => r.GetByIdAsync(book.Id)).ReturnsAsync(book);
        _memberRepository.Setup(r => r.GetByIdAsync(member.Id)).ReturnsAsync(member);
        _borrowingRepository.Setup(r => r.GetActiveBorrowCountForMemberAsync(member.Id)).ReturnsAsync(3);

        var request = new BorrowRequest { BookId = book.Id, MemberId = member.Id };

        await Assert.ThrowsAsync<BusinessRuleException>(() => _sut.BorrowAsync(request));
    }

    [Fact]
    public async Task ReturnAsync_WhenAlreadyReturned_ThrowsBusinessRuleException()
    {
        var book = new Book("Domain-Driven Design", "Eric Evans", "9780321125217", 2003, 2);
        var member = new Member("Dana Lee", "dana.lee@example.com", null);
        var borrowing = new Borrowing(book.Id, member.Id);
        borrowing.ReturnBook();

        _borrowingRepository.Setup(r => r.GetByIdAsync(borrowing.Id)).ReturnsAsync(borrowing);

        await Assert.ThrowsAsync<BusinessRuleException>(() => _sut.ReturnAsync(borrowing.Id));
    }
}
