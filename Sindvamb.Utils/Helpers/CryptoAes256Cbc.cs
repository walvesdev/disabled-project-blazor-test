#region

using System.IO;
using System.Security.Cryptography;
using Sindvamb.Utils.Extension;
using Sindvamb.Utils.Filters.CustomExceptions;

#endregion

namespace Sindvamb.Utils.Helpers;

public static class CryptoAes256Cbc
{
    private static readonly string _key = "ZyIXeJ4tvuUeDEjCrib0Guq.umRCwzjw";

    public static string Encrypt(string json, string key = null)
    {
        // Check arguments.
        if (string.IsNullOrEmpty(json)) throw new CustomException(json);

        if (key.IsNullOrWhiteSpace()) key = _key;

        var keyArr = Encoding.UTF8.GetBytes(key);
        byte[] encrypted;
        var jsonCrip = "";

        // Create an Aes object with the specified key with IV always byte[16] with 0.
        using (var aesAlg = Aes.Create())
        {
            aesAlg.Key = keyArr;
            aesAlg.IV = new byte[16];

            // Create an encryptor to perform the stream transform.
            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for encryption.
            using (var msEncrypt = new MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(json);
                    }

                    encrypted = msEncrypt.ToArray();
                }
            }
        }

        //Convert the encripted byte[] to base64String
        jsonCrip = Convert.ToBase64String(encrypted);

        return jsonCrip;
    }

    public static string Decrypt(string jsonCrip, string key = null)
    {
        // Check arguments.
        if (string.IsNullOrEmpty(jsonCrip)) throw new CustomException(jsonCrip);

        if (string.IsNullOrEmpty(key)) key = _key;

        var keyArr = Encoding.UTF8.GetBytes(key);
        var encrypted = Convert.FromBase64String(jsonCrip);
        var json = "";

        // Create an Aes object with the specified key with IV always byte[16] with 0.
        using (var aesAlg = Aes.Create())
        {
            aesAlg.Key = keyArr;
            aesAlg.IV = new byte[16];

            // Create a decryptor to perform the stream transform.
            var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for decryption.
            using (var msDecrypt = new MemoryStream(encrypted))
            {
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (var srDecrypt = new StreamReader(csDecrypt))
                    {
                        // Read the decrypted bytes from the decrypting stream and place them in a string.
                        json = srDecrypt.ReadToEnd();
                    }
                }
            }
        }

        return json;
    }
}
