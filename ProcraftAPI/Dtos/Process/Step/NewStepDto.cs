namespace ProcraftAPI.Dtos.Process.Step;

public record NewStepDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartForecast { get; set; }
    public DateTime FinishForecast { get; set; }
}
