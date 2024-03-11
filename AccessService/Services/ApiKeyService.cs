using AccessService.Data;
using AccessService.Data.Entities;
using AccessService.Models.ResponseModels;
using Microsoft.EntityFrameworkCore;

namespace AccessService.Services
{
    public class ApiKeyService : IApiKeyService
    {
        private readonly DataContext _dataContext;

        public ApiKeyService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<string> GenerateApiKey(Guid userId, List<string> permissions)
        {
            var key = Guid.NewGuid().ToString().Replace("-", string.Empty);

            var existingPermissions = await _dataContext.Permissions
                .Where(p => permissions.Contains(p.Name))
                .ToDictionaryAsync(p => p.Name, p => p);

            var permissionsToAdd = permissions
                .Where(p => !existingPermissions.ContainsKey(p))
                .Select(p => new Permission { Name = p });

            var apiKey = new ApiKey
            {
                Id = Guid.NewGuid(),
                Key = key,
                UserId = userId,
                Permissions = existingPermissions.Values.Union(permissionsToAdd).ToList(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _dataContext.AddAsync(apiKey);
            await _dataContext.SaveChangesAsync();

            return key;
        }

        public Task<bool> IsValidApiKey(string apiKey)
        {
            return _dataContext.ApiKeys.AnyAsync(k => k.Key == apiKey && k.IsActive);
        }

        public Task<ApiKey?> GetApiKey(string apiKey) 
        {
            return _dataContext.ApiKeys
                .Include(k => k.Permissions)
                .FirstOrDefaultAsync(k => k.Key == apiKey && k.IsActive);
        }

        public Task UpdateLastUsage(ApiKey apiKey, DateTime lastUsedAt) 
        {
            apiKey.LastUsedAt = lastUsedAt;
            return _dataContext.SaveChangesAsync();
        }

        public async Task RevokeApiKey(Guid apiKeyId, Guid userId)
        {
            var key = await _dataContext.ApiKeys.FindAsync(apiKeyId);

            if (key?.UserId != userId)
            {
                throw new UnauthorizedAccessException($"Cannot revoke api key '{apiKeyId}' for current user");
            }

            key.IsActive = false;
            await _dataContext.SaveChangesAsync();
        }

        public async Task<List<ApiKeyItemResponse>> GetApiKeys(Guid userId) 
        {
            var keys = await _dataContext.ApiKeys
                .Where(k => k.UserId == userId)
                .Select(k => new ApiKeyItemResponse
                {
                    Id = k.Id,
                    Key = $"...{k.Key.Substring(k.Key.Length - 4)}",
                    IsActive = k.IsActive,
                    LastUsedAt = k.LastUsedAt
                })
                .ToListAsync();

            return keys;
        }
    }
}
