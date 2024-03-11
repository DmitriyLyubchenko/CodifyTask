namespace AccessService.Services
{
    public class UserService : IUserService
    {
        public Task<Guid?> Authenticate(Guid userId, string password)
        {
            return Task.FromResult<Guid?>(userId);
        }
    }
}
