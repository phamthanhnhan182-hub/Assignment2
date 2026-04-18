using Microsoft.AspNetCore.Mvc;

namespace Assignment2_Exer4_Final.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login() => View();
        [HttpPost]
        public IActionResult Login(string user, string pass)
        {
            if (user == "admin" && pass == "123") return RedirectToAction("Index", "Home");
            return View();
        }
    }
}
