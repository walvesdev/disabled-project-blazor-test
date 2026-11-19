



using Sindvamb.Domain;




using Sindvamb.Domain.Entities;





namespace Sindvamb.Domain.Entities
{
    public class UserStatus : EntityBase<Guid>
    {

        public string Code { get; set; }

        public string Description { get; set; }

        public virtual ICollection<User> User { get; set; }
    }
}
