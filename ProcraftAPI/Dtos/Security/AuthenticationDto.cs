using ProcraftAPI.Security.Enums;

namespace ProcraftAPI.Dtos.Security;

public class AuthenticationDto
{
    public string Email { get; init; } = string.Empty;
    public string Token { get; init; } = string.Empty;
    public UserRole Role { get; init; }
    public AccountStatus AccountStatus { get; init; }
    public Guid UserId { get; init; }
};
