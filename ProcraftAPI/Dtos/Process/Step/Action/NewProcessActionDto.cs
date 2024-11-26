namespace ProcraftAPI.Dtos.Process.Step.Action;

public class NewProcessActionDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public Guid StepId { get; set; }
}
