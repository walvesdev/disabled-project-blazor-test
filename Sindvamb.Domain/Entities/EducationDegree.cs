using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindvamb.Domain.Entities
{
    public class EducationDegree : EntityBase<Guid>
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
