namespace ProcraftAPI.Entities.Process.Scope;

public class ProcessScope
{
    public Guid Id { get; set; }
    public List<ScopeAbility> Abilities { get; set; } = new List<ScopeAbility>();
}
