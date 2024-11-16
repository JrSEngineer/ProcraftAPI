using ProcraftAPI.Dtos.Process.Ability;

namespace ProcraftAPI.Dtos.Process.Scope;

public record ScopeDto
{
    public Guid Id { get; init; }
    public List<ScopeAbilityDto> Abilities { get; set; } = new List<ScopeAbilityDto>();
}
