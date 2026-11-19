using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sindvamb.Domain.Entities;

namespace Sindvamb.Domain.DTO
{
    public class SpecialNeedsDTO 
    {
        public Guid Id { get; set; }
        public string Description { get; set; }

        public virtual Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}
