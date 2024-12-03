namespace ProcraftAPI.Dtos.User.Manager;

public record NewManagerDto
{
    public Guid GroupId { get; init; }
    public Guid UserId { get; init; }
}
