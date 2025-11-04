using Common.Core.Primitives;

namespace EnigmaVault.Desktop.Services.Managers
{
    internal interface ITokenManager
    {
        Maybe<string> GetToken();
        void SaveToken(string token);
    }
}