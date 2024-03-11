using AccessService.Models.RequestModels;
using AccessService.Models.ResponseModels;

namespace AccessService.Services
{
    public interface IAuthenticationService
    {
        Task<ApiKeyResponse> GenerateApiKey(CreateApiKeyRequest model);

        Task<TokenResponse> GenerateToken(string apiKey, DateTime date);

        Task RevokeApiKey(Guid apiKeyId, Guid userId);

        Task<List<ApiKeyItemResponse>> GetApiKeys(Guid userId);
    }
}
