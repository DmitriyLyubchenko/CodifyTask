namespace AccessService.Models.RequestModels
{
    public class CreateApiKeyRequest : AuthenticatedUserRequest
    {
        public required List<string> Permissions { get; set; }
    }
}
