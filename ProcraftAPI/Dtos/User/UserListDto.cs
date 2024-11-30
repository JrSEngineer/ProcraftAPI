using ProcraftAPI.Dtos.Process.Step.Action;
using ProcraftAPI.Dtos.Process.Step;

namespace ProcraftAPI.Dtos.User;

public record UserListDto
{
    public Guid Id { get; init; }
    public string ProfileImage { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public Guid GroupId { get; init; }
    public string Cpf { get; init; } = string.Empty;
}
