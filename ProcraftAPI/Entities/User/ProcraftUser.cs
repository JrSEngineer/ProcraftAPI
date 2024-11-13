﻿using ProcraftAPI.Security.Authentication;

namespace ProcraftAPI.Entities.User;

public class ProcraftUser
{
    public Guid Id { get; set; }
    public string ProfileImage { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public ProcraftAuthentication Authentication { get; set; } = null!;
    public UserAddress Address { get; set; } = null!;
}