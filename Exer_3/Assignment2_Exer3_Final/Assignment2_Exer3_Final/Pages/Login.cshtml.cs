using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Assignment2_Exer3_Final.Pages
{
    public class LoginModel : PageModel
    {
        public void OnGet()
        {
        }

        
        public IActionResult OnPost(string user, string pass)
        {
            if (user == "admin" && pass == "123")
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }
    } 
} 