using ProcraftAPI.Enums;

namespace ProcraftAPI.Dtos.Process.Step.Action;

public record UpdateActionDto
{
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public Progress Progress { get; init; }
}
