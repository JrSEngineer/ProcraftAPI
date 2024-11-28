using ProcraftAPI.Dtos.Process;
using ProcraftAPI.Dtos.Process.Step;
using ProcraftAPI.Dtos.Process.Step.Action;
using ProcraftAPI.Dtos.Security;
using ProcraftAPI.Dtos.User.Address;

namespace ProcraftAPI.Dtos.User;

public record UserDto
{
    public Guid Id { get; init; }
    public string ProfileImage { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public Guid GroupId { get; init; }
    public string Cpf { get; init; } = string.Empty;
    public AddressDto Address { get; init; } = null!;
    public AuthenticationDto Authentication { get; init; } = null!;

    public List<ProcessListDto>? Processes { get; init; } = new List<ProcessListDto>();
    public List<StepListDto>? Steps { get; init; } = new List<StepListDto>();
    public List<ActionDto>? Actions { get; init; } = new List<ActionDto>();
}