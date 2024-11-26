namespace ProcraftAPI.Entities.User;

public class UserAddress
{
    public Guid Id { get; set; }
    public string Street { get; set; } = string.Empty;
    public int AddressNumber { get; set; }
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public Guid UserId { get; set; }
}
