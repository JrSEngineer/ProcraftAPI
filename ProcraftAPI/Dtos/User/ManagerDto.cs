namespace ProcraftAPI.Dtos.User;

public class ManagerDto
{
    public Guid ManagerId { get; init; }
    public Guid ProcessId { get; init; }
    public string ProfileImage { get; init; } = string.Empty;
    public string Email {  get; init; } = string.Empty;
}
