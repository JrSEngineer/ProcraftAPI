namespace ProcraftAPI.Dtos.Process.Step.Action;

public record FinishActionDto
{
    public Guid userId { get; init; }
    public Guid actionId { get; init; }
    public DateTime FinishedAt { get; init; }
}
