using ProcraftAPI.Enums;

namespace ProcraftAPI.Entities.Process.Step
{
    public class ProcessAction
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Progress Progress { get; set; } = Progress.Created;
        public DateTime StartedAt { get; set; }
        public DateTime FinishedAt { get; set; }
        public DateTime Duration { get; set; }
        public Guid UserId { get; set; }
        public Guid StepId { get; set; }
    }
}
