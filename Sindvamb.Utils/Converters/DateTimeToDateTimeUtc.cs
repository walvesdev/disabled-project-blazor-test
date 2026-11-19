using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Sindvamb.Utils.Converters;

public class DateTimeToDateTimeUtc : ValueConverter<DateTime, DateTime>
{
    public DateTimeToDateTimeUtc() : base(c => DateTime.SpecifyKind(c, DateTimeKind.Utc), c => c)
    {

    }
}
