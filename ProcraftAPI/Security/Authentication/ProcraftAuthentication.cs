using ProcraftAPI.Security.Enums;

namespace ProcraftAPI.Security.Authentication;

public class ProcraftAuthentication
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.user;

    public AccountStatus AccountStatus { get; set; } = AccountStatus.basic;

    public Guid UserId { get; set; }
}
