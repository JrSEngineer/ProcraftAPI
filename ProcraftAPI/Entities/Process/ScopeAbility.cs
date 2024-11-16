namespace ProcraftAPI.Entities.Process;

public class ScopeAbility
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid ScopeId { get; set; }
}