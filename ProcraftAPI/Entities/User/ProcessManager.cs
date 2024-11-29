using ProcraftAPI.Entities.Process;

namespace ProcraftAPI.Entities.User;

public class ProcessManager
{
    public Guid Id { get; set; }
    public List<ProcraftProcess> Processes { get; set; } = new List<ProcraftProcess>();
    public string ProfileImage { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
