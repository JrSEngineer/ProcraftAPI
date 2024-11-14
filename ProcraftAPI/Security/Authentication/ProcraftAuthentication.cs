using ProcraftAPI.Security.Enums;

namespace ProcraftAPI.Security.Authentication;

public class ProcraftAuthentication
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.User;

    public AccountStatus AccountStatus { get; set; } = AccountStatus.Basic;

    public Guid UserId { get; set; }
}
