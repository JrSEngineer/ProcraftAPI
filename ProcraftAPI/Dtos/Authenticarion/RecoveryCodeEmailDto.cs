namespace ProcraftAPI.Dtos.Authenticarion;

public record RecoveryCodeEmailDto
{
    public string Email { get; init; } = string.Empty;
}
