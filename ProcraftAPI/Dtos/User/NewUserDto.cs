using ProcraftAPI.Dtos.Security;
using ProcraftAPI.Dtos.User.Address;
using System.ComponentModel;

namespace ProcraftAPI.Dtos.User
{
    public record NewUserDto
    {

        [DefaultValue("João da Silva")]
        public string FullName { get; init; } = string.Empty;

        [DefaultValue("https://picsum.photos/224")]
        public string ProfileImage { get; init; } = string.Empty;

        [DefaultValue("Desenvolvedor de software com experiência em aplicações web.")]
        public string Description { get; init; } = string.Empty;

        [DefaultValue("+55 11 91234-5678")]
        public string PhoneNumber { get; init; } = string.Empty;

        [DefaultValue("46275428922")]
        public string Cpf { get; init; } = string.Empty;

        public Guid GroupId { get; init; }

        [DefaultValue(typeof(NewAuthenticationDto))]
        public NewAuthenticationDto Authentication { get; init; } = null!;

        [DefaultValue(typeof(NewUserAddressDto))]
        public NewUserAddressDto Address { get; init; } = null!;
    }
}
