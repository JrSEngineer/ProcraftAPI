using ProcraftAPI.Entities.Joins;
using ProcraftAPI.Entities.Process;
using ProcraftAPI.Entities.Process.Step;
using ProcraftAPI.Security.Authentication;

namespace ProcraftAPI.Entities.User;

public class ProcraftUser
{
    public Guid Id { get; set; }
    public string ProfileImage { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public ProcraftAuthentication Authentication { get; set; } = null!;
    public UserAddress Address { get; set; } = null!;
    public List<ProcessUser> ProcessesUsers { get; set; } = new List<ProcessUser>();
    public List<ProcraftProcess> Processes { get; set; } = new List<ProcraftProcess>();
    public List<StepUser> StepUsers { get; set; } = new List<StepUser>();
    public List<ProcessStep>? Steps { get; set; } = new List<ProcessStep>();
    public List<ProcessAction>? Actions { get; set; } = new List<ProcessAction>();
}
