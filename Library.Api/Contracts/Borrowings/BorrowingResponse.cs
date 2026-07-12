using Library.Api.Domain.Enums;

namespace Library.Api.Contracts.Borrowings;

public class BorrowingResponse
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public string? BookTitle { get; set; }
    public Guid MemberId { get; set; }
    public string? MemberName { get; set; }
    public DateTime BorrowedDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnedDate { get; set; }
    public BorrowingStatus Status { get; set; }
}
