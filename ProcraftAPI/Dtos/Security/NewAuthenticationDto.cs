using ProcraftAPI.Security.Enums;

namespace ProcraftAPI.Dtos.Security;

public record NewAuthenticationDto
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public UserRole Role { get; init; }
    public AccountStatus AccountStatus { get; init; }
}
