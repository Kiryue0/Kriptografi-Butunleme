using Microsoft.AspNetCore.Mvc;

public class AesController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Encrypt(string plainText, string password)
    {
        if (string.IsNullOrWhiteSpace(plainText) || string.IsNullOrWhiteSpace(password))
        {
            ViewBag.EncryptError = "Lütfen metin ve şifre alanlarını doldurun.";
        }
        else
        {
            try
            {
                string encryptedText = AesHelper.Encrypt(plainText, password);
                ViewBag.Encrypted = encryptedText;
            }
            catch
            {
                ViewBag.EncryptError = "Şifreleme sırasında bir hata oluştu.";
            }
        }

        // Bu bilgileri her durumda koru
        ViewBag.Input = plainText;
        ViewBag.Password = password;

        return View("Index");
    }

    [HttpPost]
    public IActionResult Decrypt(string encryptedText, string password)
    {
        if (string.IsNullOrWhiteSpace(encryptedText) || string.IsNullOrWhiteSpace(password))
        {
            ViewBag.DecryptError = "Lütfen şifreli metin ve anahtar girin.";
        }
        else
        {
            try
            {
                string decryptedText = AesHelper.Decrypt(encryptedText, password);
                ViewBag.Decrypted = decryptedText;
            }
            catch
            {
                ViewBag.DecryptError = "Şifre çözme başarısız. Girilen anahtar yanlış olabilir.";
            }
        }

        // Bu bilgileri koru (özellikle üst alanlar boşalmasın)
        ViewBag.Encrypted = encryptedText;
        ViewBag.Password = password;

        // Geriye ViewBag.Input değerini korumak için varsa en son girilen metni ViewData'dan ya da TempData'dan çekebilirsin.
        // Ama burada input alanı boş kalmasın diye varsayılan olarak aşağıya bir değer aktaralım (isteğe bağlı):
        ViewBag.Input = ""; // Boş bırak ya da istersen son şifrelenmiş metni ver

        return View("Index");
    }
}
