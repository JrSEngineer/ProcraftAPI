using ProcraftAPI.Entities.Process.Step;
using ProcraftAPI.Entities.User;

namespace ProcraftAPI.Entities.Joins
{
    public class StepUser
    {
        public Guid UserId { get; set; }
        public ProcraftUser User { get; set; } = null!;
        public Guid StepId { get; set; }
        public ProcessStep Step { get; set; } = null!;
    }
}
