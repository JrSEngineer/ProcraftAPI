namespace ProcraftAPI.Dtos.User.Manager;

public record ManagerListDto
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string ProfileImage { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}
