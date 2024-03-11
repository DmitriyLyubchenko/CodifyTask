namespace AccessService.Data.Entities
{
    public class ApiKey
    {
        public Guid Id { get; set; }

        public required string Key { get; set; }

        public Guid UserId { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? LastUsedAt { get; set; }

        public List<Permission>? Permissions { get; set; }
    }
}
