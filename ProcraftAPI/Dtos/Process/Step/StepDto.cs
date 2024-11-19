﻿using ProcraftAPI.Enums;

namespace ProcraftAPI.Dtos.Process.Step;

public record StepDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public Progress Progress { get; init; }
    public DateTime StartForecast { get; init; }
    public DateTime FinishForecast { get; init; }
    public Guid ProcessId { get; init; }
    public List<ActionDto>? Actions { get; init; } = new List<ActionDto>();
}