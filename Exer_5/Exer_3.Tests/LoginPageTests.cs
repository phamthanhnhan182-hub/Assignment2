using Assignment2_Exer3_Final.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Exer_3.Tests
{
    [TestClass]
    public class LoginPageTests
    {
        [TestMethod]
        public void LoginModel_OnPost_ValidCredentials_RedirectsToIndex()
        {
            var pageModel = new LoginModel();

            var result = pageModel.OnPost("admin", "123");

            var redirect = result as RedirectToPageResult;
            Assert.IsNotNull(redirect);
            Assert.AreEqual("/Index", redirect.PageName);
        }

        [DataTestMethod]
        [DataRow("admin", "123")]
        [DataRow(" admin ", "123")]
        public void LoginModel_OnPost_InputComparison_BehavesAsExpected(string user, string pass)
        {
            var pageModel = new LoginModel();

            var result = pageModel.OnPost(user, pass);

            if (user == "admin" && pass == "123")
            {
                Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            }
            else
            {
                Assert.IsInstanceOfType(result, typeof(PageResult));
            }
        }

        [DataTestMethod]
        [DataRow("admin", "wrong")]
        [DataRow("user", "123")]
        [DataRow("", "")]
        [DataRow(null, null)]
        public void LoginModel_OnPost_InvalidCredentials_ReturnsPage(string user, string pass)
        {
            var pageModel = new LoginModel();

            var result = pageModel.OnPost(user, pass);

            Assert.IsInstanceOfType(result, typeof(PageResult));
        }
    }
}
