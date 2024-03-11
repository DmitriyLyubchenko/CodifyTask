namespace AccessService.Configuration
{
    public class JwtTokenSettings
    {
        public required string Key { get; set; }

        public required string Issuer { get; set; }

        public int Lifetime { get; set; }
    }
}
