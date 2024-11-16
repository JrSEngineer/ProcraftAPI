﻿using ProcraftAPI.Dtos.User;
using ProcraftAPI.Enums;

namespace ProcraftAPI.Dtos.Process;

public record ProcessDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public Progress Progress { get; init; }
    public List<UserListDto> Users { get; set; } = [];
}