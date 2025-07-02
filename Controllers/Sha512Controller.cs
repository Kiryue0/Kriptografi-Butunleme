using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

public class Sha512Controller : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult TextHash(string inputText)
    {
        if (string.IsNullOrWhiteSpace(inputText))
        {
            ViewBag.HashError = "Lütfen metin girin.";
            return View("Index");
        }

        using (SHA512 sha512 = SHA512.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputText);
            byte[] hashBytes = sha512.ComputeHash(inputBytes);
            string hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            ViewBag.TextInput = inputText;
            ViewBag.HashResult = hashString;
        }

        return View("Index");
    }

    [HttpPost]
    public IActionResult FileHash(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            ViewBag.HashError = "Lütfen bir dosya seçin.";
            return View("Index");
        }

        using (SHA512 sha512 = SHA512.Create())
        using (var stream = file.OpenReadStream())
        {
            byte[] hashBytes = sha512.ComputeHash(stream);
            string hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();

            ViewBag.FileName = file.FileName;
            ViewBag.FileHashResult = hashString;
        }

        return View("Index");
    }
}
