namespace ProcraftAPI.Security.Authentication;

public class AccountRecovery
{
    public Guid TransactionId { get; set; }
    public string VerificationCode { get; set; } = string.Empty;
    public string RecoveryEmail { get; set; } = string.Empty;
    public bool CodeUsedInPastOperation { get; set; } = false;
    public DateTime SendedAt { get; set; }
}
