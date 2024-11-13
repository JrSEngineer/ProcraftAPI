using ProcraftAPI.Dtos.Security;
using ProcraftAPI.Dtos.User.Address;

namespace ProcraftAPI.Dtos.User
{
    public record NewUserDto
    {
        public string FullName { get; init; } = string.Empty;
        public string ProfileImage { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string PhoneNumber { get; init; } = string.Empty;
        public string Cpf { get; init; } = string.Empty;
        public NewAuthenticationDto Authentication { get; init; } = null!;
        public NewUserAddressDto Address { get; init; } = null!;
    }
}
