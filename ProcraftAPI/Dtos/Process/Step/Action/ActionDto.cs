using ProcraftAPI.Enums;

namespace ProcraftAPI.Dtos.Process.Step.Action
{
    public class ActionDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Progress Progress { get; set; } = Progress.Created;
        public TimeSpan? Duration { get; set; }
        public Guid UserId { get; set; }
        public Guid StepId { get; set; }
    }
}
