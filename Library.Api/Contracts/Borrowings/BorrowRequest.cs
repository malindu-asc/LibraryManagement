using System.ComponentModel.DataAnnotations;

namespace Library.Api.Contracts.Borrowings;

public class BorrowRequest
{
    [Required]
    public Guid BookId { get; set; }

    [Required]
    public Guid MemberId { get; set; }
}
