namespace Library.Api.Contracts.Members;

public class MemberResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    public DateTime RegisteredDate { get; set; }
    public bool IsActive { get; set; }
}
