using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models; // Thay bằng tên Project của bạn

namespace WebApplication1.Controllers
{
    public partial class AccountController : Controller
    {
        // Khai báo thực thể Database (Tên này giống trong file Model của bạn)
        private SE_LabAssignment2Entities2 db = new SE_LabAssignment2Entities2();

        // 1. Action hiển thị trang Login (GET)
        public ActionResult Login()
        {
            return View();
        }

        // 2. Action xử lý khi nhấn nút Login (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            if (ModelState.IsValid)
            {
                // Tìm tài khoản trong bảng Users
                var user = db.Users.FirstOrDefault(u => u.UserName.Equals(username) && u.Password.Equals(password));

                if (user != null)
                {
                    // Đăng nhập thành công: Lưu thông tin vào Session
                    Session["UserID"] = user.UserID;
                    Session["UserName"] = user.UserName;

                    // Chuyển về trang chủ (Main Form)
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Đăng nhập thất bại
                    ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng!";
                }
            }
            return View();
        }

        // 3. Action Đăng xuất
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}