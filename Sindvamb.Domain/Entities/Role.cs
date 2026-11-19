

using Newtonsoft.Json;


using Sindvamb.Domain;
using Sindvamb.Domain.Entities;

namespace Sindvamb.Domain.Entities
{
    public class Role : EntityBase<Guid>
    {
        
        
        public Role()
        {
            Users = new HashSet<User>();
        }


        public string Key { get; set; }

        public string Value { get; set; }
        public string Description { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
