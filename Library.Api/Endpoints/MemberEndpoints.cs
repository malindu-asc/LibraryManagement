using Library.Api.Application.Interfaces;
using Library.Api.Contracts.Members;

namespace Library.Api.Endpoints;

public static class MemberEndpoints
{
    public static void MapMemberEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/members").WithTags("Members");

        group.MapPost("/", async (CreateMemberRequest request, IMemberService memberService) =>
        {
            var member = await memberService.CreateAsync(request);
            return Results.Created($"/api/members/{member.Id}", member);
        })
        .AddEndpointFilter<ValidationFilter<CreateMemberRequest>>()
        .WithName("RegisterMember")
        .Produces<MemberResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();

        group.MapGet("/", async (IMemberService memberService) =>
            Results.Ok(await memberService.GetAllAsync()))
        .WithName("GetMembers")
        .Produces<List<MemberResponse>>();

        group.MapGet("/{id:guid}", async (Guid id, IMemberService memberService) =>
            Results.Ok(await memberService.GetByIdAsync(id)))
        .WithName("GetMemberById")
        .Produces<MemberResponse>()
        .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/{id:guid}", async (Guid id, UpdateMemberRequest request, IMemberService memberService) =>
            Results.Ok(await memberService.UpdateAsync(id, request)))
        .AddEndpointFilter<ValidationFilter<UpdateMemberRequest>>()
        .WithName("UpdateMember")
        .Produces<MemberResponse>()
        .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{id:guid}", async (Guid id, IMemberService memberService) =>
        {
            await memberService.DeleteAsync(id);
            return Results.NoContent();
        })
        .WithName("DeleteMember")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);
    }
}
