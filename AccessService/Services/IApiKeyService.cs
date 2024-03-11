using AccessService.Data.Entities;
using AccessService.Models.ResponseModels;

namespace AccessService.Services
{
    public interface IApiKeyService
    {
        Task<string> GenerateApiKey(Guid userId, List<string> permissions);

        Task<bool> IsValidApiKey(string apiKey);

        Task<ApiKey?> GetApiKey(string apiKey);

        Task UpdateLastUsage(ApiKey apiKey, DateTime lastUsedAt);

        Task RevokeApiKey(Guid apiKeyId, Guid userId);

        Task<List<ApiKeyItemResponse>> GetApiKeys(Guid userId);
    }
}
