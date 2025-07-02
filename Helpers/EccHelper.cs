using System;
using System.Security.Cryptography;
using System.Text;

public static class EccHelper
{
    // ECC anahtar çifti oluşturur ve public key'i Base64 formatında döner
    public static (CngKey PrivateKey, string PublicKeyBase64) GenerateKeyPair()
    {
        var privateKey = CngKey.Create(CngAlgorithm.ECDiffieHellmanP256);
        byte[] publicKeyBlob = privateKey.Export(CngKeyBlobFormat.EccPublicBlob);
        string publicKeyBase64 = Convert.ToBase64String(publicKeyBlob);
        return (privateKey, publicKeyBase64);
    }

    // Paylaşılan ortak anahtarı üretir
    public static byte[] DeriveSharedKey(CngKey privateKey, string otherPublicKeyBase64)
    {
        byte[] otherPublicKeyBlob = Convert.FromBase64String(otherPublicKeyBase64);
        var otherPublicKey = CngKey.Import(otherPublicKeyBlob, CngKeyBlobFormat.EccPublicBlob);

        using var ecdh = new ECDiffieHellmanCng(privateKey)
        {
            KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash,
            HashAlgorithm = CngAlgorithm.Sha256
        };

        return ecdh.DeriveKeyMaterial(otherPublicKey);
    }

    // AES ile şifreleme
    public static string EncryptWithAes(string plainText, byte[] aesKey, out byte[] iv)
    {
        using var aes = Aes.Create();
        aes.Key = aesKey;
        aes.GenerateIV();
        iv = aes.IV;

        var encryptor = aes.CreateEncryptor();
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] encrypted = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        return Convert.ToBase64String(encrypted);
    }

    // AES ile deşifreleme
    public static string DecryptWithAes(string encryptedBase64, byte[] aesKey, byte[] iv)
    {
        byte[] encrypted = Convert.FromBase64String(encryptedBase64);
        using var aes = Aes.Create();
        aes.Key = aesKey;
        aes.IV = iv;

        var decryptor = aes.CreateDecryptor();
        byte[] decrypted = decryptor.TransformFinalBlock(encrypted, 0, encrypted.Length);
        return Encoding.UTF8.GetString(decrypted);
    }
}
