namespace AccessService.Data.Entities
{
    public class Permission
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public List<ApiKey>? ApiKeys { get; set; }
    }
}
