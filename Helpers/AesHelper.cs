using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class AesHelper
{
    public static string Encrypt(string plainText, string password)
    {
        byte[] key = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        byte[] iv = new byte[16]; // 128 bit IV (varsayılan 0)

        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        using var encryptor = aes.CreateEncryptor();
        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        using var sw = new StreamWriter(cs);
        sw.Write(plainText);
        sw.Close();

        return Convert.ToBase64String(ms.ToArray());
    }

    public static string Decrypt(string encryptedText, string password)
    {
        byte[] key = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        byte[] iv = new byte[16];

        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor();
        using var ms = new MemoryStream(Convert.FromBase64String(encryptedText));
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);
        return sr.ReadToEnd();
    }
}
