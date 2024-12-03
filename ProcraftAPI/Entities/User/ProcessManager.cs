using ProcraftAPI.Entities.Process;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcraftAPI.Entities.User;

public class ProcessManager
{
    public Guid Id { get; set; }
    public string ProfileImage { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid GroupId { get; set; }
    [ForeignKey("User")]
    public Guid UserId { get; set; }
    public ProcraftUser User { get; set; } = null!;
    public List<ProcraftProcess> Processes { get; set; } = new List<ProcraftProcess>();
}
