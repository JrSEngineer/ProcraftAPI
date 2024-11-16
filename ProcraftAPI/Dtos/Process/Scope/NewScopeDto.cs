using ProcraftAPI.Dtos.Process.Ability;

namespace ProcraftAPI.Dtos.Process.Scope;

public record NewScopeDto
{
    public List<NewScopeAbilityDto> Abilities { get; set; } = new List<NewScopeAbilityDto>();
}
