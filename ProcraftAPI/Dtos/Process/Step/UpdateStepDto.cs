using ProcraftAPI.Enums;

namespace ProcraftAPI.Dtos.Process.Step;

public record UpdateStepDto
{
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public Progress Progress { get; init; }
}