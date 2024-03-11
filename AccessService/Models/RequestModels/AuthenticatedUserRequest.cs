namespace AccessService.Models.RequestModels
{
    public class AuthenticatedUserRequest
    {
        public Guid UserId { get; set; }

        public required string Password { get; set; }
    }
}
