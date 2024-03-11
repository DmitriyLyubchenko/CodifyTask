namespace AccessService.Services
{
    public interface IUserService
    {
        Task<Guid?> Authenticate(Guid userId, string password);
    }
}
