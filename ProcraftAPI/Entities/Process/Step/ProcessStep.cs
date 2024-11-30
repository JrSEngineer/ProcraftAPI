using ProcraftAPI.Entities.Joins;
using ProcraftAPI.Entities.User;
using ProcraftAPI.Enums;

namespace ProcraftAPI.Entities.Process.Step;

public class ProcessStep
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Progress Progress { get; set; } = Progress.Created;
    public DateTime StartForecast { get; set; }
    public DateTime FinishForecast { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public Guid ProcessId { get; set; }
    public ProcraftProcess Process { get; set; } = null!;
    public List<ProcessAction>? Actions { get; set; } = new List<ProcessAction>();
    public List<StepUser> StepUsers { get; set; } = new List<StepUser>();
    public List<ProcraftUser> Users { get; set; } = new List<ProcraftUser>();
}
