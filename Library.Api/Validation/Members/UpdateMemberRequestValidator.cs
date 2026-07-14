using FluentValidation;
using Library.Api.Contracts.Members;

namespace Library.Api.Validation.Members;

public class UpdateMemberRequestValidator : AbstractValidator<UpdateMemberRequest>
{
    public UpdateMemberRequestValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Email).NotEmpty().MaximumLength(200).EmailAddress();
    }
}
