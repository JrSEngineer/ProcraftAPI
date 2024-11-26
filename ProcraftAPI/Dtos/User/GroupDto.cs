namespace ProcraftAPI.Dtos.User;

public record GroupDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public List<UserListDto> Members { get; init; } = new List<UserListDto>();
}
