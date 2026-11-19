#region

using Hangfire;

#endregion

namespace Sindvamb.HangFire.Jobs.Extensions;

public static class RecurringJobs
{
    public static void AddRecurringJobs(this IRecurringJobManager recurringJobManager)
    {
        #region Transfere

        // Job que incia o processo de trânsferencia de arquivos
        // recurringJobManager.AddOrUpdate<TransfereScheduleJob>("Transfer:TransferScheduleJob", m =>
        //     m.Execute(null, null, JobCancellationToken.Null), "*/30 * * * *", TimeZoneInfo.Local);

        #endregion
    }
}