using ProcraftAPI.Enums;
namespace ProcraftAPI.Dtos.Process;

public class ProcessListDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public Progress Progress { get; init; }
    public double FinishedStepsPorcentage { get; init; }
}
