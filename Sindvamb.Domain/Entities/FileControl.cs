

using Sindvamb.Domain;
using Sindvamb.Domain.Entities;



namespace Sindvamb.Domain.Entities
{
    public class FileControl : EntityBase<Guid>
    {
        
        public string FileName { get; set; }
        public decimal FileSize { get; set; }
        public byte[] FileArray { get; set; }
        public string? TargetPath { get; set; }
        public string ContentType { get; set; }
        public string? Description { get; set; }
        public bool? Approved { get; set; }

        public virtual Guid? DependentId { get; set; }

        public virtual Guid? UserId { get; set; }

        public Guid? LayoutId { get; set; }
        public virtual FileLayout Layout { get; set; }
    }
}
