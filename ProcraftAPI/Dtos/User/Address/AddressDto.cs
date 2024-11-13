namespace ProcraftAPI.Dtos.User.Address;

public class AddressDto
{
    public Guid Id { get; init; }
    public string Street { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;
    public string ZipCode { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public Guid UserId { get; init; }
}
