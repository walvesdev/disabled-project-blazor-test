#region

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sindvamb.Utils.Crypto;

#endregion

namespace Sindvamb.Utils.Converters;

public class EncryptedConverter : ValueConverter<string, string>
{
    public EncryptedConverter(ConverterMappingHints mappingHints = default)
        : base(enc => Cryptographer.Encrypt(enc), dec => Cryptographer.Decrypt(dec), mappingHints)
    {
    }
}
