using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace EnigmaVault.Desktop.Services.Secure
{
    public sealed class SecureDataService : ISecureDataService
    {
        private const int NonceSize = 12;
        private const int TagSize = 16;

        private const int Iterations = 600_000;
        private const int KeySize = 32;

        public (byte[] Kek, string AuthHash) DeriveKeysFromPassword(string password, byte[] salt)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            byte[] masterKey = pbkdf2.GetBytes(KeySize); 

            byte[] kek = HKDF.DeriveKey(
                HashAlgorithmName.SHA256,
                masterKey,
                outputLength: KeySize,
                info: Encoding.UTF8.GetBytes("AES-GCM-KEK-v1")
            );

            byte[] authBytes = HKDF.DeriveKey(
                HashAlgorithmName.SHA256,
                masterKey,
                outputLength: KeySize,
                info: Encoding.UTF8.GetBytes("SERVER-AUTH-HASH-v1")
            );

            Array.Clear(masterKey, 0, masterKey.Length);

            string authHashString = Convert.ToBase64String(authBytes);

            return (kek, authHashString);
        }

        public string EncryptData<T>(T dataModel, byte[] key)
        {
            string jsonString = JsonSerializer.Serialize(dataModel);
            byte[] plainBytes = Encoding.UTF8.GetBytes(jsonString);

            var nonce = new byte[NonceSize];
            RandomNumberGenerator.Fill(nonce);

            var cipherText = new byte[plainBytes.Length];
            var tag = new byte[TagSize];

            using (var aes = new AesGcm(key, TagSize))
            {
                aes.Encrypt(nonce, plainBytes, cipherText, tag);
            }

            var result = new byte[NonceSize + cipherText.Length + TagSize];
            Buffer.BlockCopy(nonce, 0, result, 0, NonceSize);
            Buffer.BlockCopy(cipherText, 0, result, NonceSize, cipherText.Length);
            Buffer.BlockCopy(tag, 0, result, NonceSize + cipherText.Length, TagSize);

            return Convert.ToBase64String(result);
        }

        public T? DecryptData<T>(string encryptedBase64, byte[] key)
        {
            if (string.IsNullOrEmpty(encryptedBase64)) return default;

            byte[] encryptedBytes = Convert.FromBase64String(encryptedBase64);

            if (encryptedBytes.Length < NonceSize + TagSize)
                throw new ArgumentException("Недопустимый формат зашифрованных данных");

            var nonce = encryptedBytes.AsSpan(0, NonceSize);
            var tag = encryptedBytes.AsSpan(encryptedBytes.Length - TagSize, TagSize);
            var cipherText = encryptedBytes.AsSpan(NonceSize, encryptedBytes.Length - NonceSize - TagSize);

            var plainBytes = new byte[cipherText.Length];

            using (var aes = new AesGcm(key, TagSize))
            {
                aes.Decrypt(nonce, cipherText, tag, plainBytes);
            }

            string jsonString = Encoding.UTF8.GetString(plainBytes);

            return JsonSerializer.Deserialize<T>(jsonString);
        }

        public byte[] GenerateRandomBytes(int length = 32)
        {
            var bytes = new byte[length];
            RandomNumberGenerator.Fill(bytes);
            return bytes;
        }
    }
}