using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace KriptografiWebApp.Controllers
{
    public class Sha256Controller : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult TextHash(string inputText)
        {
            if (string.IsNullOrEmpty(inputText))
                return RedirectToAction("Index");

            using var sha256 = SHA256.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputText);
            byte[] hashBytes = sha256.ComputeHash(inputBytes);
            string hashResult = Convert.ToHexString(hashBytes);

            ViewBag.TextInput = inputText;
            ViewBag.HashResult = hashResult;
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> FileHash(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return RedirectToAction("Index");

            using var sha256 = SHA256.Create();
            using var stream = file.OpenReadStream();
            byte[] hashBytes = await sha256.ComputeHashAsync(stream);
            string hashResult = Convert.ToHexString(hashBytes);

            ViewBag.FileName = file.FileName;
            ViewBag.FileHashResult = hashResult;
            return View("Index");
        }
    }
}
