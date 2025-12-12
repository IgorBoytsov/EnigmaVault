namespace EnigmaVault.Desktop.Services.Secure
{
    public interface ISecureDataService
    {
        (byte[] Kek, string AuthHash) DeriveKeysFromPassword(string password, byte[] salt);
        T? DecryptData<T>(string encryptedBase64, byte[] key);
        string EncryptData<T>(T dataModel, byte[] key);
        byte[] GenerateRandomBytes(int length = 32);
    }
}