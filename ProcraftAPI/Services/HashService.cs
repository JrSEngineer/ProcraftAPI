using ProcraftAPI.Interfaces;
using System.Security.Cryptography;

namespace ProcraftAPI.Services
{
    public class HashService : IHashService
    {
        private readonly int saltSize;
        private readonly int keySize;
        private readonly int iterations;
        private readonly HashAlgorithmName algorithmName;
        private readonly char segmentDelimiter;

        public HashService(
            int saltSize,
            int keySize,
            int iterations,
            HashAlgorithmName algorithmName,
            char segmentDelimiter
            )
        {
            this.saltSize = saltSize;
            this.keySize = keySize;
            this.iterations = iterations;
            this.algorithmName = algorithmName;
            this.segmentDelimiter = segmentDelimiter;
        }

        public string HashValue(string value)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(saltSize);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                value,
                salt,
                iterations,
                algorithmName,
                keySize
            );

            string newHashValue = string.Join(
                segmentDelimiter,
                Convert.ToHexString(hash),
                Convert.ToHexString(salt),
                iterations,
                algorithmName
            );

            return newHashValue;
        }

        public bool CompareValue(string value, string hashedValue)
        {
            string[] segments = hashedValue.Split(segmentDelimiter);

            byte[] hash = Convert.FromHexString(segments[0]);

            byte[] salt = Convert.FromHexString(segments[1]);

            int iterations = int.Parse(segments[2]);

            HashAlgorithmName algorithm = new HashAlgorithmName(segments[3]);

            byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(
                value,
                salt,
                iterations,
                algorithm,
                hash.Length
            );

            bool hashValuesMatching = CryptographicOperations.FixedTimeEquals(inputHash, hash);

            return hashValuesMatching;
        }

        public string GenerateVerificationCode(int codeSize)
        {
            var codeBaseString = Guid.NewGuid().ToString().Split("-");

            var generatedCode = codeBaseString
                .First()
                .Substring(0, codeSize);

            return generatedCode;
        }
    }
}
