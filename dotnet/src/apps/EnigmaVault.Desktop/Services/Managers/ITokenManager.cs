using Common.Core.Primitives;
using EnigmaVault.Desktop.Models;
using Shared.Contracts.Responses;

namespace EnigmaVault.Desktop.Services.Managers
{
    internal interface ITokenManager
    {
        //Maybe<string> GetToken();
        //void SaveToken(string token);
        void SaveTokens(AccessData tokens);
        Maybe<AccessData> GetTokens();
        void ClearTokens();
    }
}