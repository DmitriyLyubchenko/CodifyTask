using AccessService.Models.RequestModels;
using AccessService.Models.ResponseModels;

namespace AccessService.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IApiKeyService _apiKeyService;
        private readonly IJwtService _jwtService;

        public AuthenticationService(IApiKeyService apiKeyService, IJwtService jwtService) 
        {
            _apiKeyService = apiKeyService;
            _jwtService = jwtService;
        }

        public async Task<ApiKeyResponse> GenerateApiKey(CreateApiKeyRequest model) 
        {
            var apiKey = await _apiKeyService.GenerateApiKey(model.UserId, model.Permissions);

            return new ApiKeyResponse { ApiKey = apiKey };
        }

        public async Task<TokenResponse> GenerateToken(string apiKey, DateTime date) 
        {
            var key = await _apiKeyService.GetApiKey(apiKey);

            if (key == null)
            {
                throw new UnauthorizedAccessException("Wrong Api Key");
            }

            var permissions = key.Permissions.Select(x => x.Name).ToList();
            var token = _jwtService.CreateToken(key.UserId, permissions);

            await _apiKeyService.UpdateLastUsage(key, date);

            return new TokenResponse { Token = token };
        }

        public Task RevokeApiKey(Guid apiKeyId, Guid userId) 
        {
            return _apiKeyService.RevokeApiKey(apiKeyId, userId);
        }

        public Task<List<ApiKeyItemResponse>> GetApiKeys(Guid userId) 
        {
            return _apiKeyService.GetApiKeys(userId);
        }
    }
}
