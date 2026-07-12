using Library.Api.Application.Interfaces;
using Library.Api.Contracts.Borrowings;

namespace Library.Api.Endpoints;

public static class BorrowingEndpoints
{
    public static void MapBorrowingEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/borrowings").WithTags("Borrowings");

        group.MapPost("/", async (BorrowRequest request, IBorrowingService borrowingService) =>
        {
            var borrowing = await borrowingService.BorrowAsync(request);
            return Results.Created($"/api/borrowings/{borrowing.Id}", borrowing);
        })
        .AddEndpointFilter<ValidationFilter<BorrowRequest>>()
        .WithName("BorrowBook")
        .Produces<BorrowingResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();

        group.MapGet("/", async (IBorrowingService borrowingService) =>
            Results.Ok(await borrowingService.GetAllAsync()))
        .WithName("GetBorrowings")
        .Produces<List<BorrowingResponse>>();

        group.MapPost("/{id:guid}/return", async (Guid id, IBorrowingService borrowingService) =>
            Results.Ok(await borrowingService.ReturnAsync(id)))
        .WithName("ReturnBook")
        .Produces<BorrowingResponse>()
        .Produces(StatusCodes.Status404NotFound);

        app.MapGet("/api/members/{memberId:guid}/borrowings", async (Guid memberId, IBorrowingService borrowingService) =>
            Results.Ok(await borrowingService.GetByMemberAsync(memberId)))
        .WithTags("Borrowings")
        .WithName("GetBorrowingsByMember")
        .Produces<List<BorrowingResponse>>()
        .Produces(StatusCodes.Status404NotFound);
    }
}
