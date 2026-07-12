using Library.Api.Application.Interfaces;
using Library.Api.Contracts.Borrowings;
using Library.Api.Domain.Entities;
using Library.Api.Domain.Enums;
using Library.Api.Domain.Exceptions;

namespace Library.Api.Application.Services;

public class BorrowingService : IBorrowingService
{
    private const int MaxActiveBorrowings = 3;

    private readonly IBorrowingRepository _borrowingRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IMemberRepository _memberRepository;

    public BorrowingService(
        IBorrowingRepository borrowingRepository,
        IBookRepository bookRepository,
        IMemberRepository memberRepository)
    {
        _borrowingRepository = borrowingRepository;
        _bookRepository = bookRepository;
        _memberRepository = memberRepository;
    }

    public async Task<BorrowingResponse> BorrowAsync(BorrowRequest request)
    {
        var book = await _bookRepository.GetByIdAsync(request.BookId)
            ?? throw new NotFoundException("Book not found.");

        var member = await _memberRepository.GetByIdAsync(request.MemberId)
            ?? throw new NotFoundException("Member not found.");

        if (!member.IsActive)
            throw new BusinessRuleException("Inactive member cannot borrow books.");

        var activeCount = await _borrowingRepository.GetActiveBorrowCountForMemberAsync(member.Id);
        if (activeCount >= MaxActiveBorrowings)
            throw new BusinessRuleException("Member has reached the maximum of 3 active borrowed books.");

        if (book.AvailableCopies <= 0)
            throw new BusinessRuleException("Book is unavailable.");

        book.BorrowCopy();
        var borrowing = new Borrowing(book.Id, member.Id);

        _bookRepository.Update(book);
        await _borrowingRepository.AddAsync(borrowing);
        await _borrowingRepository.SaveChangesAsync();

        var saved = await _borrowingRepository.GetByIdAsync(borrowing.Id);

        return ToResponse(saved!);
    }

    public async Task<List<BorrowingResponse>> GetAllAsync()
    {
        var borrowings = await _borrowingRepository.GetAllAsync();
        return borrowings.Select(ToResponse).ToList();
    }

    public async Task<List<BorrowingResponse>> GetByMemberAsync(Guid memberId)
    {
        var member = await _memberRepository.GetByIdAsync(memberId)
            ?? throw new NotFoundException("Member not found.");

        var borrowings = await _borrowingRepository.GetByMemberIdAsync(member.Id);
        return borrowings.Select(ToResponse).ToList();
    }

    public async Task<BorrowingResponse> ReturnAsync(Guid borrowingId)
    {
        var borrowing = await _borrowingRepository.GetByIdAsync(borrowingId)
            ?? throw new NotFoundException("Borrowing record not found.");

        if (borrowing.Status == BorrowingStatus.Returned)
            throw new BusinessRuleException("This book has already been returned.");

        var book = await _bookRepository.GetByIdAsync(borrowing.BookId)
            ?? throw new NotFoundException("Book not found.");

        borrowing.ReturnBook();
        book.ReturnCopy();

        _borrowingRepository.Update(borrowing);
        _bookRepository.Update(book);
        await _borrowingRepository.SaveChangesAsync();

        return ToResponse(borrowing);
    }

    private static BorrowingResponse ToResponse(Borrowing borrowing) => new()
    {
        Id = borrowing.Id,
        BookId = borrowing.BookId,
        BookTitle = borrowing.Book?.Title,
        MemberId = borrowing.MemberId,
        MemberName = borrowing.Member?.FullName,
        BorrowedDate = borrowing.BorrowedDate,
        DueDate = borrowing.DueDate,
        ReturnedDate = borrowing.ReturnedDate,
        Status = borrowing.Status
    };
}
