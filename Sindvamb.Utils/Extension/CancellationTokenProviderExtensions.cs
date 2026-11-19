using System.Threading;
using Sindvamb.Utils.Types;

namespace Sindvamb.Utils.Extension
{
    public static class CancellationTokenProviderExtensions
    {
        public static CancellationToken FallbackToProvider(this ICancellationTokenProvider provider,
            CancellationToken prefferedValue = default) =>
            prefferedValue == default || prefferedValue == CancellationToken.None
                ? provider.Token
                : prefferedValue;

        public static void ForceCancel(ref this CancellationToken cancellationToken,
            Func<bool>? condition = null)
        {
            if (condition == null || condition.Invoke())
            {
                var cts = CancellationTokenSource.CreateLinkedTokenSource(
                    cancellationToken);
                cancellationToken = cts.Token;
                cts.Cancel();
            }
        }
    }
}
