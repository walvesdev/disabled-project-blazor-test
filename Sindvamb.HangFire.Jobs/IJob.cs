#region

using Hangfire;
using Hangfire.Server;

#endregion

namespace Sindvamb.HangFire.Jobs;

public interface IJob
{
    Task<T> Execute<T>(object? obj, PerformContext? context, IJobCancellationToken cancellationToken);
    Task Execute(object? obj, PerformContext? context, IJobCancellationToken cancellationToken);
}