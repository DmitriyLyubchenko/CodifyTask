namespace AccessService.Models.RequestModels
{
    public class RevokeApiKeyRequest : AuthenticatedUserRequest
    {
        public Guid ApiKeyId { get; set; }
    }
}
