using System.ComponentModel.DataAnnotations;

namespace ProcraftAPI.Dtos.Authenticarion;

public record RecoveryPasswordDto
{
    [Length(8, 124, ErrorMessage = "Please, provide a password with at least 8 characters.")]
    public string Password { get; set; } = string.Empty;
    public string ConfirmationPassword { get; set; } = string.Empty;
    public string RecoveryEmail { get; set; } = string.Empty;
}
