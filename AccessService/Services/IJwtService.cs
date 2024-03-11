namespace AccessService.Services
{
    public interface IJwtService
    {
        string CreateToken(Guid userId, List<string> permissions);
    }
}
