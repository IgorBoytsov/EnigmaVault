using Common.Core.Primitives;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EnigmaVault.Desktop.Services.Managers
{
    internal sealed class TokenManager : ITokenManager
    {
        private readonly string _filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EnigmaVault", "token.data");
        private static readonly byte[] s_entropy = Encoding.Unicode.GetBytes("3yl0uJIFfN7jI432aUJe23wrFXWhygCQ");

        public void SaveToken(string token)
        {
            byte[] tokenBytes = Encoding.Unicode.GetBytes(token);

            byte[] encryptedBytes = ProtectedData.Protect(tokenBytes, s_entropy, DataProtectionScope.CurrentUser);

            Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);

            File.WriteAllBytes(_filePath, encryptedBytes);
        }

        public Maybe<string> GetToken()
        {
            if (!File.Exists(_filePath))
                return null;

            try
            {
                byte[] encryptedBytes = File.ReadAllBytes(_filePath);

                byte[] decryptedBytes = ProtectedData.Unprotect(encryptedBytes, s_entropy, DataProtectionScope.CurrentUser);

                return Encoding.Unicode.GetString(decryptedBytes);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
