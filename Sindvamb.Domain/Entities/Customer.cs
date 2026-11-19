#nullable disable

using Sindvamb.Domain;
using Sindvamb.Domain.DTO;
using Sindvamb.Domain.Entities;

#nullable disable

namespace Sindvamb.Domain.Entities
{
    public class Customer : EntityBaseAudit<Guid>
    {

        public Customer()
        {
            CustomerOrder = new HashSet<CustomerOrder>();
            Dependents = new HashSet<Dependent>();
        }

        public virtual Guid UserId { get; set; }
        public virtual User User { get; set; }

        public virtual Guid? CustomerTypeId { get; set; }
        public virtual CustomerType CustomerType { get; set; }

        public virtual ICollection<CustomerOrder> CustomerOrder { get; set; }

        public virtual ICollection<Dependent> Dependents { get; set; }

        public Customer GetFilesWithouFilesStream()
        {
            if (User?.FileControl is not null)
            {
                User = User.GetUserWithouFilesStream();
            }

            if (Dependents is null) return this;

            foreach (var Dependents in Dependents)
            {
                if (Dependents.FileControl is null) return this;

                foreach (var file in Dependents.FileControl)
                {
                    file.FileArray = null;
                }
            }

            return this;
        }
    }
}
