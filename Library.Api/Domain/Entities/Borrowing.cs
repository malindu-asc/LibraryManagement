using Library.Api.Domain.Enums;

namespace Library.Api.Domain.Entities;

public class Borrowing
{
    public Guid Id { get; private set; }
    public Guid BookId { get; private set; }
    public Guid MemberId { get; private set; }
    public DateTime BorrowedDate { get; private set; }
    public DateTime DueDate { get; private set; }
    public DateTime? ReturnedDate { get; private set; }
    public BorrowingStatus Status { get; private set; }

    public Book? Book { get; private set; }
    public Member? Member { get; private set; }

    private Borrowing() { }

    public Borrowing(Guid bookId, Guid memberId)
    {
        Id = Guid.NewGuid();
        BookId = bookId;
        MemberId = memberId;
        BorrowedDate = DateTime.UtcNow;
        DueDate = BorrowedDate.AddDays(14);
        Status = BorrowingStatus.Borrowed;
    }

    public void ReturnBook()
    {
        if (Status == BorrowingStatus.Returned)
            throw new InvalidOperationException("This book has already been returned.");

        ReturnedDate = DateTime.UtcNow;
        Status = BorrowingStatus.Returned;
    }
}
