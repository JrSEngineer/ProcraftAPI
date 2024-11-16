using ProcraftAPI.Entities.Joins;
using ProcraftAPI.Entities.User;
using ProcraftAPI.Enums;
namespace ProcraftAPI.Entities.Process;

public class ProcraftProcess
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Progress Progress { get; set; } = Progress.Created;
    public DateTime StartForecast { get; set; }
    public DateTime ConclusionForecast { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public List<ProcessUser> ProcessesUsers { get; set; } = new List<ProcessUser>();
    public List<ProcraftUser> Users { get; set; } = new List<ProcraftUser>();
    public ProcessScope? Scope { get; set; }

}
