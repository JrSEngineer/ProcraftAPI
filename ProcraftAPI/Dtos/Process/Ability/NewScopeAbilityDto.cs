namespace ProcraftAPI.Dtos.Process.Ability;

public record NewScopeAbilityDto
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}