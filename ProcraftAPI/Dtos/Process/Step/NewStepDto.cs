using System.ComponentModel;

namespace ProcraftAPI.Dtos.Process.Step;

public record NewStepDto
{

    [DefaultValue("Planejamento")]
    public string Title { get; set; } = string.Empty;

    [DefaultValue("Definir funcionalidades e layout do aplicativo.")]
    public string Description { get; set; } = string.Empty;

    public DateTime StartForecast { get; set; }

    public DateTime FinishForecast { get; set; }
}
