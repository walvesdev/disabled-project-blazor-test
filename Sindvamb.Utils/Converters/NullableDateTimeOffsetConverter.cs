using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindvamb.Utils.Converters
{
    public class NullableDateTimeOffsetConverter : ValueConverter<DateTimeOffset?, DateTimeOffset?>
    {
        public NullableDateTimeOffsetConverter()
            : base(
                d => d == null ? null : d.Value.ToUniversalTime(),
                d => d == null ? null : d.Value.ToUniversalTime())
        {
        }
    }
}
