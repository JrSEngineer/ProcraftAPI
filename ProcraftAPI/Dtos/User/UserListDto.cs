namespace ProcraftAPI.Dtos.User;

public record UserListDto
{
    public Guid Id { get; init; }
    public string ProfileImage { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public string Cpf { get; init; } = string.Empty;
}
