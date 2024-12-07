namespace ProcraftAPI.Dtos.Process.Ability;

public record ScopeAbilityDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public Guid ScopeId { get; init; }
}