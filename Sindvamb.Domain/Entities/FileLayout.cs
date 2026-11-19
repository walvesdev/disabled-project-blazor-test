

using Sindvamb.Domain;


using Sindvamb.Domain.Entities;



namespace Sindvamb.Domain.Entities
{

    public class FileLayout : EntityBase<Guid>
    {
        

        public string LayoutName { get; set; }

        public short LayoutSize { get; set; }


        public virtual ICollection<FileControl> FileControl { get; set; }

    }

}
