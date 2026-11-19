using Sindvamb.Utils.Types;

namespace Sindvamb.Utils.Types;

public sealed class NullDisposable : IDisposable
{
    private NullDisposable()
    {
    }

    public static NullDisposable Instance { get; } = new();

    public void Dispose()
    {
    }
}
