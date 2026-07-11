using System.ComponentModel.DataAnnotations;

namespace Library.Api.Contracts.Members;

public class CreateMemberRequest
{
    [Required]
    public string FullName { get; set; } = default!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;

    public string? PhoneNumber { get; set; }
}
