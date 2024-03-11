namespace AccessService.Models.ResponseModels
{
    public class ApiKeyItemResponse
    {
        public Guid Id { get; set; }

        public required string Key { get; set; }

        public bool IsActive { get; set; }

        public DateTime? LastUsedAt { get; set; }
    }
}
