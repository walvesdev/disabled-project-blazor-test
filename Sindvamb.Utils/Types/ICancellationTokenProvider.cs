using System.Threading;

namespace Sindvamb.Utils.Types
{
    public interface ICancellationTokenProvider
    {
        CancellationToken Token { get; }

        IDisposable Use(CancellationToken cancellationToken);
    }
}
