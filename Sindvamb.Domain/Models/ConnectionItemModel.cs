using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindvamb.Domain.Models
{
    public class ConnectionItemModel
    {
        [NotNull]
        public string? Id { get; set; }

        public dynamic ClientInfo { get; set; }
      
        public DateTimeOffset ConnectionTime { get; set; }

        public DateTimeOffset LastBeatTime { get; set; }
    }
}
