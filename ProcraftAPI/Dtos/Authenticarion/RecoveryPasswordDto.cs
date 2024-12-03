namespace ProcraftAPI.Dtos.Authenticarion;

public record RecoveryPasswordDto
{
    public string Password { get; set; } = string.Empty;
    public string ConfirmationPassword { get; set; } = string.Empty;
    public string RecoveryCode {  get; set; } = string.Empty;
}
