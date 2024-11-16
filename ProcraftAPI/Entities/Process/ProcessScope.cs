namespace ProcraftAPI.Entities.Process;

public class ProcessScope
{
    public Guid Id { get; set; }
    public List<ScopeAbility> Abilities { get; set; } = new List<ScopeAbility>();
}
