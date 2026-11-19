namespace Sindvamb.Caching;

public interface IDistributedCacheKeyNormalizer
{
    string NormalizeKey(DistributedCacheKeyNormalizeArgs args);
}
