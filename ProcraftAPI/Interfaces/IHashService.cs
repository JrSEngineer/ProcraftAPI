namespace ProcraftAPI.Interfaces
{
    public interface IHashService
    {
        public string HashValue(string value);

        public bool CompareValue(string value, string hashedValue);

        public string GenerateVerificationCode(int codeSize);
    }
}
