using FluentValidation;
using Library.Api.Contracts.Borrowings;

namespace Library.Api.Validation.Borrowings;

public class BorrowRequestValidator : AbstractValidator<BorrowRequest>
{
    public BorrowRequestValidator()
    {
        RuleFor(x => x.BookId).NotEmpty();
        RuleFor(x => x.MemberId).NotEmpty();
    }
}
