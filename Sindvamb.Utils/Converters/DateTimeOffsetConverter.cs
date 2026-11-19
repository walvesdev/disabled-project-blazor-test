using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindvamb.Utils.Converters
{
    public class DateTimeOffsetConverter : ValueConverter<DateTimeOffset, DateTimeOffset>
    {
        public DateTimeOffsetConverter()
            : base(
                d => d.ToUniversalTime(),
                d => d.ToUniversalTime())
        {
        }
    }
}
