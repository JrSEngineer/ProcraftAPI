namespace ProcraftAPI.Entities.User;

public class ProcraftGroup
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public List<ProcraftUser> Members { get; set; } = new List<ProcraftUser>();
}
