using System.ComponentModel.DataAnnotations;
using ProcraftAPI.Dtos.Process.Scope;

namespace ProcraftAPI.Dtos.Process;

public record NewProcessDto
{
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime StartForecast { get; init; }
    public DateTime FinishForecast { get; init; }
    public List<UserIdDto> Users { get; init; } = new List<UserIdDto>();
    public NewScopeDto? ScopeDto { get; init; }
}
