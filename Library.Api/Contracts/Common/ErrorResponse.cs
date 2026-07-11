namespace Library.Api.Contracts.Common;

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = default!;
    public string? TraceId { get; set; }
}

public class ValidationErrorResponse
{
    public int StatusCode { get; set; } = 400;
    public string Message { get; set; } = "Validation failed";
    public List<ValidationErrorItem> Errors { get; set; } = new();
}

public class ValidationErrorItem
{
    public string Field { get; set; } = default!;
    public string Message { get; set; } = default!;
}
