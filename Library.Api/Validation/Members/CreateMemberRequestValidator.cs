using FluentValidation;
using Library.Api.Contracts.Members;

namespace Library.Api.Validation.Members;

public class CreateMemberRequestValidator : AbstractValidator<CreateMemberRequest>
{
    public CreateMemberRequestValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Email).NotEmpty().MaximumLength(200).EmailAddress();
    }
}
