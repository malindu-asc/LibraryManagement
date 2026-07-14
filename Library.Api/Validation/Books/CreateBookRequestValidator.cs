using FluentValidation;
using Library.Api.Contracts.Books;

namespace Library.Api.Validation.Books;

public class CreateBookRequestValidator : AbstractValidator<CreateBookRequest>
{
    public CreateBookRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Author).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Isbn).NotEmpty().MaximumLength(20);
        RuleFor(x => x.PublishedYear).GreaterThan(0);
        RuleFor(x => x.TotalCopies).GreaterThan(0).WithMessage("TotalCopies must be greater than 0.");
    }
}
