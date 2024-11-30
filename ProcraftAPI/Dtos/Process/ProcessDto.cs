using ProcraftAPI.Dtos.Process.Scope;
using ProcraftAPI.Dtos.Process.Step;
using ProcraftAPI.Dtos.User;
using ProcraftAPI.Enums;

namespace ProcraftAPI.Dtos.Process;

public record ProcessDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public Progress Progress { get; init; }
    public DateTime StartForecast { get; init; }
    public DateTime FinishForecast { get; init; }
    public DateTime? StartedAt { get; init; }
    public DateTime? FinishedAt { get; init; }
    public ManagerDto Manager { get; init; } = null!;
    public List<UserListDto> Users { get; init; } = new List<UserListDto>();
    public ScopeDto? Scope { get; init; }
    public List<StepListDto> Steps { get; init; } = new List<StepListDto>();
}
