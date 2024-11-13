using ProcraftAPI.Security.Authentication;

namespace ProcraftAPI.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(ProcraftAuthentication authentication);

        public bool ValidateToken(string token);
    }
}
