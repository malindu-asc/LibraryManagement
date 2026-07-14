using FluentValidation;
using Library.Api.Contracts.Common;

namespace Library.Api.Endpoints;

// [Required]/[EmailAddress]/[Range] remain on the request DTOs solely so Swashbuckle
// can populate the Swagger schema (required fields, formats, ranges) - FluentValidation
// below is what actually enforces validation at request time.
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

        var validator = context.HttpContext.RequestServices.GetRequiredService<IValidator<T>>();
        var result = await validator.ValidateAsync(model);

        if (!result.IsValid)
        {
            var errors = result.Errors.Select(e => new ValidationErrorItem
            {
                Field = e.PropertyName,
                Message = e.ErrorMessage
            }).ToList();

            return Results.BadRequest(new ValidationErrorResponse { Errors = errors });
        }

        return await next(context);
    }
}
