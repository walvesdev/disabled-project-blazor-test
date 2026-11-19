#region

using System.IO;
using System.Security.Cryptography;

#endregion

namespace Sindvamb.Utils.Cryptography;

public static class Cryptographer
{
    private const string DEFAULT_PASS_PHRASE = "36BFA7D5-E75C-45B5-8FE0-77A195CD80E2";

    private const string INITIAL_VECTOR = "@1B2c3D4e5F6g7H8";

    private const string PRIVATE_KEY = "OAB42DB268133DA4D1190605D36FB22246DOAB";

    private const string HASH_ALGORITHM = "SHA1";

    private const int PASSWORD_ITERATIONS = 2;

    private const int KEY_SIZE = 256;

    //
    // Resumo:
    //     Decriptografar texto
    //
    // Parâmetros:
    //   value:
    //     texto
    public static string Decrypt(string value) => Decrypt(value, "36BFA7D5-E75C-45B5-8FE0-77A195CD80E2");

    //
    // Resumo:
    //     Decriptografar texto
    //
    // Parâmetros:
    //   value:
    //     texto
    //
    //   passPhrase:
    //     senha para decriptografia
    //
    // Devoluções:
    //     Caso consiga decriptografar retorna o texto, caso contrário retorna String.Empty
    public static string Decrypt(string value, string passPhrase)
    {
        var bytes = Encoding.ASCII.GetBytes("@1B2c3D4e5F6g7H8");
        var bytes2 = Encoding.ASCII.GetBytes("OAB42DB268133DA4D1190605D36FB22246DOAB");
        var array = Convert.FromBase64String(value);
        using var passwordDeriveBytes = new PasswordDeriveBytes(passPhrase, bytes2, "SHA1", 2);
        var bytes3 = passwordDeriveBytes.GetBytes(32);
        using var rijndaelManaged = new RijndaelManaged();
        rijndaelManaged.Mode = CipherMode.CBC;
        var transform = rijndaelManaged.CreateDecryptor(bytes3, bytes);
        using var stream = new MemoryStream(array);
        using var cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Read);
        var array2 = new byte[array.Length];
        var count = cryptoStream.Read(array2, 0, array2.Length);
        return Encoding.UTF8.GetString(array2, 0, count);
    }

    //
    // Resumo:
    //     Criptografar texto
    //
    // Parâmetros:
    //   value:
    //     texto
    public static string Encrypt(string value) => Encrypt(value, "36BFA7D5-E75C-45B5-8FE0-77A195CD80E2");

    //
    // Resumo:
    //     Criptografar texto
    //
    // Parâmetros:
    //   value:
    //     texto
    //
    //   passPhrase:
    //     senha para criptografia
    //
    // Devoluções:
    //     Texto criptografado
    public static string Encrypt(string value, string passPhrase)
    {
        var bytes = Encoding.ASCII.GetBytes("@1B2c3D4e5F6g7H8");
        var bytes2 = Encoding.ASCII.GetBytes("OAB42DB268133DA4D1190605D36FB22246DOAB");
        var bytes3 = Encoding.UTF8.GetBytes(value);
        using var passwordDeriveBytes = new PasswordDeriveBytes(passPhrase, bytes2, "SHA1", 2);
        var bytes4 = passwordDeriveBytes.GetBytes(32);
        using var rijndaelManaged = new RijndaelManaged();
        rijndaelManaged.Mode = CipherMode.CBC;
        var transform = rijndaelManaged.CreateEncryptor(bytes4, bytes);
        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
        cryptoStream.Write(bytes3, 0, bytes3.Length);
        cryptoStream.FlushFinalBlock();
        return Convert.ToBase64String(memoryStream.ToArray());
    }

    //
    // Resumo:
    //     Gerar Hash MD5
    //
    // Parâmetros:
    //   value:
    //     valor
    public static string GetMD5Hash(string value)
    {
        var mD5CryptoServiceProvider = MD5.Create();
        var bytes = Encoding.Default.GetBytes(value);
        return BitConverter.ToString(mD5CryptoServiceProvider.ComputeHash(bytes));
    }

    //
    // Resumo:
    //     Gera uma senha randômica
    //
    // Parâmetros:
    //   lengthOfPassword:
    //     Tamanho da senha
    //
    //   includeLowercase:
    //     Indica se a senha vai conter caracteres minúsculos
    //
    //   includeUppercase:
    //     Indica se a senha vai conter caracteres maiúsculos
    //
    //   includeNumeric:
    //     Indica se a senha vai conter caracteres numéricos
    //
    //   includeSpecial:
    //     Indica se a senha vai conter caracteres especiais
    //
    //   includeSpaces:
    //     Indica se a senha vai conter espaços
    public static string GeneratePassword(int lengthOfPassword, bool includeLowercase = true, bool includeUppercase = true, bool includeNumeric = true, bool includeSpecial = true, bool includeSpaces = false)
    {
        if (lengthOfPassword < 8 || lengthOfPassword > 128) return "Password length must be between 8 and 128.";

        var text = "";
        if (includeLowercase) text += "abcdefghijklmnopqrstuvwxyz";

        if (includeUppercase) text += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        if (includeNumeric) text += "0123456789";

        if (includeSpecial) text += "!#$%&*@\\";

        if (includeSpaces) text += " ";

        var array = new char[lengthOfPassword];
        var length = text.Length;
        var random = new Random();
        for (var i = 0; i < lengthOfPassword; i++)
        {
            array[i] = text[random.Next(length - 1)];
            if (i > 2 && array[i] == array[i - 1] && array[i - 1] == array[i - 2]) i--;
        }

        return string.Join(null, array);
    }

    //
    // Resumo:
    //     Criar uma assinatura desacoplada do conteúdo
    //
    // Parâmetros:
    //   content:
    //     Conteúdo a ser assinado
    //
    //   certificate:
    //     Certificado utilizado para assinar
    //
    //   value:
    //     texto
    //
    // Devoluções:
    //     Evelope de assinatura
    public static string EncryptToHex(string value)
    {
        var s = Encrypt(value);
        return BitConverter.ToString(Encoding.UTF8.GetBytes(s)).Replace("-", "");
    }

    //
    // Resumo:
    //     Decriptografar texto
    //
    // Parâmetros:
    //   hex:
    //     Hexadecimal
    public static string DecryptFromHex(string hex)
    {
        var bytes = (from x in Enumerable.Range(0, hex.Length)
                     where x % 2 == 0
                     select Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
        return Decrypt(Encoding.UTF8.GetString(bytes));
    }
}
