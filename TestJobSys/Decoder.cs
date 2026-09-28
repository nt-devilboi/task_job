using System.Security.Cryptography;
using System.Text;

namespace TestJobSys;

public static class Decoder
{
    public static string DecryptAesEcb(string encryptedTextBytesB64, string keyBytesB64)
    {
        var cipher = Convert.FromBase64String(encryptedTextBytesB64);
        var key = Convert.FromBase64String(keyBytesB64);

        using var aes = Aes.Create();
        aes.Mode = CipherMode.ECB;
        aes.Padding = PaddingMode.None;
        aes.Key = key;

        using var decryptor = aes.CreateDecryptor();
        var plain = decryptor.TransformFinalBlock(cipher, 0, cipher.Length);

        return Encoding.UTF8.GetString(plain);
    }


    public static string DecodeBase64(string b64)
    {
        var bytes = Convert.FromBase64String(b64);
        return Encoding.UTF8.GetString(bytes);
    }
}