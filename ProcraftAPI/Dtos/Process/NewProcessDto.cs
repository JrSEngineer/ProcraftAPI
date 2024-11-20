using System.ComponentModel.DataAnnotations;
using ProcraftAPI.Dtos.Process.Scope;
using ProcraftAPI.Dtos.Process.Step;

namespace ProcraftAPI.Dtos.Process;

public record NewProcessDto
{
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime StartForecast { get; init; }
    public DateTime FinishForecast { get; init; }

    [MinLength(1, ErrorMessage = "Plase, provide at least 1 member the project.")]
    public List<UserIdDto> Users { get; init; } = new List<UserIdDto>();
    public NewScopeDto? Scope { get; init; }

    [MinLength(1, ErrorMessage = "Plase, provide at least 1 initial step to the project.")]
    public List<NewStepDto> Steps { get; init; } = new List<NewStepDto>();
}
