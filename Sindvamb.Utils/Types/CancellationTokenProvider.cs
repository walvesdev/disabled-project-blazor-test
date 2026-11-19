using System.Threading;
using Sindvamb.Utils.Extension;
using Sindvamb.Utils.Types;

namespace Sindvamb.Utils.Types
{
    public class CancellationTokenProvider : ICancellationTokenProvider
    {
        private CancellationToken _cancellationToken;

        public static CancellationTokenProvider Instance { get; } = new CancellationTokenProvider();

        public CancellationToken Token
        {
            get
            {
                if (_cancellationToken.IsNotNull()) return _cancellationToken;

                return new CancellationTokenSource().Token;
            }
            set => _cancellationToken = value;
        }

        public IDisposable Use(CancellationToken cancellationToken) =>
            new DisposeAction(() =>
            {
                if (cancellationToken.IsNotNull()) Token = cancellationToken;
            });
    }
}
