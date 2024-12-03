using ProcraftAPI.Entities.Joins;
using ProcraftAPI.Entities.Process.Scope;
using ProcraftAPI.Entities.Process.Step;
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
    public DateTime FinishForecast { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public List<ProcessUser> ProcessesUsers { get; set; } = new List<ProcessUser>();
    public List<ProcraftUser> Users { get; set; } = new List<ProcraftUser>();
    public ProcessScope? Scope { get; set; }
    public List<ProcessStep> Steps { get; set; } = new List<ProcessStep>();
    public ProcessManager? Manager { get; set; }
    public Guid ManagerId { get; set; }

    public double CalculateProocessProgressPorcentage()
    {
        int totalStepsCount = Steps.Count();

        if(totalStepsCount == 0)
        {
            return 0;
        }

        var finishedSteps = Steps.Where((step) => step.Progress == Progress.Finished).ToList();

        int finishedStepsCount = finishedSteps.Count() * 100;

        int finishedStepsPorcentage = finishedStepsCount / totalStepsCount;

        return (double)finishedStepsPorcentage;
    }
}
