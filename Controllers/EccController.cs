using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

public class ECCController : Controller
{
    private static CngKey UserAPrivateKey;
    private static string UserAPublicKeyBase64;
    private static CngKey UserBPrivateKey;
    private static string UserBPublicKeyBase64;

    private static byte[] SharedKeyAtoB;
    private static byte[] SharedKeyBtoA;

    private static string EncryptedMessageAtoB;
    private static string DecryptedMessageB;

    [HttpGet]
    public IActionResult Index()
    {
        if (UserAPrivateKey == null || UserBPrivateKey == null)
        {
            (UserAPrivateKey, UserAPublicKeyBase64) = EccHelper.GenerateKeyPair();
            (UserBPrivateKey, UserBPublicKeyBase64) = EccHelper.GenerateKeyPair();
        }

        // Private key gizleniyor
        ViewBag.UserAPrivateKey = "[Private Key gizlidir]";
        ViewBag.UserAPublicKey = UserAPublicKeyBase64;

        ViewBag.UserBPrivateKey = "[Private Key gizlidir]";
        ViewBag.UserBPublicKey = UserBPublicKeyBase64;

        ViewBag.SharedKeyAtoB = SharedKeyAtoB != null ? Convert.ToBase64String(SharedKeyAtoB) : null;
        ViewBag.SharedKeyBtoA = SharedKeyBtoA != null ? Convert.ToBase64String(SharedKeyBtoA) : null;

        ViewBag.EncryptedMessageAtoB = EncryptedMessageAtoB;
        ViewBag.DecryptedMessageB = DecryptedMessageB;

        return View();
    }

    [HttpPost]
    public IActionResult CreateSharedKeyA(string otherPublicKeyBase64)
    {
        try
        {
            SharedKeyAtoB = EccHelper.DeriveSharedKey(UserAPrivateKey, otherPublicKeyBase64);
            TempData["MessageA"] = "Ortak anahtar başarıyla oluşturuldu.";
        }
        catch
        {
            TempData["MessageA"] = "Ortak anahtar oluşturulamadı, lütfen geçerli bir public key girin.";
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult CreateSharedKeyB(string otherPublicKeyBase64)
    {
        try
        {
            SharedKeyBtoA = EccHelper.DeriveSharedKey(UserBPrivateKey, otherPublicKeyBase64);
            TempData["MessageB"] = "Ortak anahtar başarıyla oluşturuldu.";
        }
        catch
        {
            TempData["MessageB"] = "Ortak anahtar oluşturulamadı, lütfen geçerli bir public key girin.";
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult EncryptAtoB(string plainText)
    {
        if (SharedKeyAtoB == null)
        {
            TempData["ErrorA"] = "Önce ortak anahtar oluşturun.";
            return RedirectToAction("Index");
        }

        try
        {
            var encrypted = EccHelper.EncryptWithAes(plainText, SharedKeyAtoB, out var iv);
            EncryptedMessageAtoB = encrypted + ":" + Convert.ToBase64String(iv);
            TempData["MessageA"] = "Mesaj şifrelendi.";
        }
        catch
        {
            TempData["ErrorA"] = "Şifreleme sırasında hata oluştu.";
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult DecryptB(string encryptedMessageWithIV)
    {
        if (SharedKeyBtoA == null)
        {
            TempData["ErrorB"] = "Önce ortak anahtar oluşturun.";
            return RedirectToAction("Index");
        }

        try
        {
            var parts = encryptedMessageWithIV.Split(':');
            var encrypted = parts[0];
            var iv = Convert.FromBase64String(parts[1]);
            var decrypted = EccHelper.DecryptWithAes(encrypted, SharedKeyBtoA, iv);
            DecryptedMessageB = decrypted;
            TempData["MessageB"] = "Mesaj çözüldü.";
        }
        catch
        {
            TempData["ErrorB"] = "Şifre çözme başarısız.";
        }
        return RedirectToAction("Index");
    }
}
