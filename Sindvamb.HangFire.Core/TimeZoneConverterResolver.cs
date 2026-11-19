#region

using Hangfire;

using TimeZoneConverter;

#endregion

namespace Sindvamb.Utils.Resolvers;

public sealed class TimeZoneConverterResolver : ITimeZoneResolver
{
    public TimeZoneInfo GetTimeZoneById(string timeZoneId) => TZConvert.GetTimeZoneInfo(timeZoneId);
}