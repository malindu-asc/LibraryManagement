using Library.Api.Application.Interfaces;
using Library.Api.Contracts.Books;

namespace Library.Api.Endpoints;

public static class BookEndpoints
{
    public static void MapBookEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/books").WithTags("Books");

        group.MapPost("/", async (CreateBookRequest request, IBookService bookService) =>
        {
            var book = await bookService.CreateAsync(request);
            return Results.Created($"/api/books/{book.Id}", book);
        })
        .AddEndpointFilter<ValidationFilter<CreateBookRequest>>()
        .WithName("CreateBook")
        .Produces<BookResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();

        group.MapGet("/", async (IBookService bookService) =>
            Results.Ok(await bookService.GetAllAsync()))
        .WithName("GetBooks")
        .Produces<List<BookResponse>>();

        group.MapGet("/{id:guid}", async (Guid id, IBookService bookService) =>
            Results.Ok(await bookService.GetByIdAsync(id)))
        .WithName("GetBookById")
        .Produces<BookResponse>()
        .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/{id:guid}", async (Guid id, UpdateBookRequest request, IBookService bookService) =>
            Results.Ok(await bookService.UpdateAsync(id, request)))
        .AddEndpointFilter<ValidationFilter<UpdateBookRequest>>()
        .WithName("UpdateBook")
        .Produces<BookResponse>()
        .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{id:guid}", async (Guid id, IBookService bookService) =>
        {
            await bookService.DeleteAsync(id);
            return Results.NoContent();
        })
        .WithName("DeleteBook")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);
    }
}
