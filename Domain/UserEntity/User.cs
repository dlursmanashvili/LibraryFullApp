using Microsoft.AspNetCore.Identity;

namespace Domain.UserEntity;

public class User : IdentityUser<string>
{
    public string PNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? ConfirmationId { get; set; }
    public bool IsOorganisation { get; set; }
    public bool IsActive { get; set; } = true;
}
