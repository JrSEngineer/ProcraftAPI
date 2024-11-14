using ProcraftAPI.Entities.Process;
using ProcraftAPI.Entities.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcraftAPI.Entities.Joins;

public class ProcessUser
{
    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }
    public ProcraftUser User { get; set; } = null!;

    [ForeignKey(nameof(Process))]
    public Guid ProcessId { get; set; }
    public ProcraftProcess Process { get; set; } = null!;

}
