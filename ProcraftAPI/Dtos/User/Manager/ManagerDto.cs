using ProcraftAPI.Dtos.Process;

namespace ProcraftAPI.Dtos.User.Manager;

public class ManagerDto
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public Guid ProcessId { get; init; }
    public string ProfileImage { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public List<ProcessListDto>? Processes { get; init; }
}
