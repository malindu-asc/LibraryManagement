using System.ComponentModel.DataAnnotations;
using Library.Api.Contracts.Common;

namespace Library.Api.Endpoints;

public class ValidationFilter<T> : IEndpointFilter where T : class
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var model = context.Arguments.OfType<T>().FirstOrDefault();

        if (model is null)
            return Results.BadRequest(new ValidationErrorResponse
            {
                Errors = new() { new ValidationErrorItem { Field = "body", Message = "Request body is required." } }
            });

        var validationContext = new ValidationContext(model);
        var validationResults = new List<ValidationResult>();

        if (!Validator.TryValidateObject(model, validationContext, validationResults, validateAllProperties: true))
        {
            var errors = validationResults.Select(v => new ValidationErrorItem
            {
                Field = v.MemberNames.FirstOrDefault() ?? "",
                Message = v.ErrorMessage ?? "Invalid value."
            }).ToList();

            return Results.BadRequest(new ValidationErrorResponse { Errors = errors });
        }

        return await next(context);
    }
}
