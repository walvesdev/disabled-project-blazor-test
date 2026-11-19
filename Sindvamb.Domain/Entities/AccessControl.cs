namespace Sindvamb.Domain.Entities
{
    public class AccessControl : EntityBase<Guid>
    {
        public Guid? UserId { get; set; }

        public string? UserName { get; set; }

        public DateTimeOffset? ConnectionTime { get; set; }

        public DateTimeOffset? LastBeatTime { get; set; }

        public TimeSpan? Dur { get; set; }

        public string? Ip { get; set; }

        public string? City { get; set; }

        public string? OS { get; set; }

        public string? Device { get; set; }

        public string? Browser { get; set; }

        public string? Language { get; set; }

        public string? Engine { get; set; }

        public string? RequestUrl { get; set; }
    }
}
