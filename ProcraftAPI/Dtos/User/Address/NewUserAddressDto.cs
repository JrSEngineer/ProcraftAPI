using System.ComponentModel;
using System.Security.Cryptography;

namespace ProcraftAPI.Dtos.User.Address
{
    public class NewUserAddressDto
    {

       [DefaultValue("Rua das Flores, 123")] 
        public string Street { get; init; } = string.Empty;
        
        [DefaultValue(123)]
        public int AddressNumber { get; init; }

        [DefaultValue("São Paulo")]
        public string City { get; init; } = string.Empty;

       [DefaultValue("SP")]
        public string State { get; init; } = string.Empty;

       [DefaultValue("12345-678")]
        public string ZipCode { get; init; } = string.Empty;

       [DefaultValue("Brasil")]
        public string Country { get; init; } = string.Empty;
    }
}
