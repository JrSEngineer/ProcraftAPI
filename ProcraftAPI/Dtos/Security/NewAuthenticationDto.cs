using ProcraftAPI.Security.Enums;
using System.ComponentModel;

namespace ProcraftAPI.Dtos.Security;

public record NewAuthenticationDto
{
    [DefaultValue("joao.silva@example.com")]
    public string Email { get; init; } = string.Empty;

    [DefaultValue("senhaSegura123")]
    public string Password { get; init; } = string.Empty;

    [DefaultValue(typeof(UserRole))]
    public UserRole Role { get; init; }

    [DefaultValue(typeof(AccountStatus))]
    public AccountStatus AccountStatus { get; init; }
}
