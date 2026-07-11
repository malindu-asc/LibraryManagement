namespace Library.Api.Domain.Entities;

public class Member
{
    public Guid Id { get; private set; }
    public string FullName { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string? PhoneNumber { get; private set; }
    public DateTime RegisteredDate { get; private set; }
    public bool IsActive { get; private set; }

    private Member() { }

    public Member(string fullName, string email, string? phoneNumber)
    {
        Id = Guid.NewGuid();
        FullName = fullName;
        Email = email;
        PhoneNumber = phoneNumber;
        RegisteredDate = DateTime.UtcNow;
        IsActive = true;
    }

    public void Update(string fullName, string email, string? phoneNumber, bool isActive)
    {
        FullName = fullName;
        Email = email;
        PhoneNumber = phoneNumber;
        IsActive = isActive;
    }
}
