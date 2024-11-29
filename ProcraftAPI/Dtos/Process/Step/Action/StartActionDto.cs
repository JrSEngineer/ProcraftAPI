namespace ProcraftAPI.Dtos.Process.Step.Action;

public record StartActionDto
{
    public Guid userId { get; init; }
    public Guid actionId { get; init; }
    public DateTime StartedAt { get; init; }
}
