﻿using ProcraftAPI.Enums;

namespace ProcraftAPI.Dtos.Process;

public class UpdateProcessDto
{
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public Progress Progress { get; init; }
}
