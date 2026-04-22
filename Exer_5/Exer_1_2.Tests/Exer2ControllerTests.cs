using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Exer_1_2.Tests
{
    [TestClass]
    public class Exer2ControllerTests
    {
        [DataTestMethod]
        [DataRow("Exer_2\\WebApplication1\\WebApplication1\\Controllers\\AccountController.cs")]
        [DataRow("Exer_2\\WebApplication1\\WebApplication1\\Controllers\\OrdersController.cs")]
        public void Exer2_Controller_SourceFiles_Exist(string relativePath)
        {
            var fullPath = Path.Combine(GetRootPath(), relativePath);
            Assert.IsTrue(File.Exists(fullPath), $"Missing file: {fullPath}");
        }

        [TestMethod]
        public void AccountController_Contains_Login_Get_Action()
        {
            var source = ReadSource("Exer_2", "WebApplication1", "WebApplication1", "Controllers", "AccountController.cs");

            StringAssert.Contains(source, "public ActionResult Login()");
            StringAssert.Contains(source, "return View();");
        }

        [TestMethod]
        public void AccountController_Contains_Login_Post_Action()
        {
            var source = ReadSource("Exer_2", "WebApplication1", "WebApplication1", "Controllers", "AccountController.cs");

            StringAssert.Contains(source, "[HttpPost]");
            StringAssert.Contains(source, "[ValidateAntiForgeryToken]");
            StringAssert.Contains(source, "public ActionResult Login(string username, string password)");
        }

        [TestMethod]
        public void AccountController_Contains_Session_And_Redirect()
        {
            var source = ReadSource("Exer_2", "WebApplication1", "WebApplication1", "Controllers", "AccountController.cs");

            StringAssert.Contains(source, "Session[\"UserID\"]");
            StringAssert.Contains(source, "Session[\"UserName\"]");
            StringAssert.Contains(source, "RedirectToAction(\"Index\", \"Home\")");
            StringAssert.Contains(source, "ViewBag.Error");
        }

        [TestMethod]
        public void AccountController_Contains_Logout_Action()
        {
            var source = ReadSource("Exer_2", "WebApplication1", "WebApplication1", "Controllers", "AccountController.cs");

            StringAssert.Contains(source, "public ActionResult Logout()");
            StringAssert.Contains(source, "Session.Clear();");
            StringAssert.Contains(source, "RedirectToAction(\"Login\")");
        }

        [TestMethod]
        public void OrdersController_Contains_Create_Get_Action()
        {
            var source = ReadSource("Exer_2", "WebApplication1", "WebApplication1", "Controllers", "OrdersController.cs");

            StringAssert.Contains(source, "public ActionResult Create()");
            StringAssert.Contains(source, "ViewBag.AgentID = new SelectList");
            StringAssert.Contains(source, "ViewBag.ItemID = new SelectList");
        }

        [TestMethod]
        public void OrdersController_Contains_Create_Post_Action()
        {
            var source = ReadSource("Exer_2", "WebApplication1", "WebApplication1", "Controllers", "OrdersController.cs");

            StringAssert.Contains(source, "public ActionResult Create(Order order, int[] ItemID, int[] Quantity)");
            StringAssert.Contains(source, "order.OrderDate = DateTime.Now;");
            StringAssert.Contains(source, "db.Orders.Add(order);");
            StringAssert.Contains(source, "db.OrderDetails.Add(detail);");
        }

        [TestMethod]
        public void OrdersController_Contains_BadRequest_Guards()
        {
            var source = ReadSource("Exer_2", "WebApplication1", "WebApplication1", "Controllers", "OrdersController.cs");

            StringAssert.Contains(source, "return new HttpStatusCodeResult(HttpStatusCode.BadRequest);");
            StringAssert.Contains(source, "if (id == null)");
        }

        [TestMethod]
        public void OrdersController_Contains_Statistics_Action()
        {
            var source = ReadSource("Exer_2", "WebApplication1", "WebApplication1", "Controllers", "OrdersController.cs");

            StringAssert.Contains(source, "public ActionResult Statistics()");
            StringAssert.Contains(source, "GroupBy(d => d.Item.ItemName)");
            StringAssert.Contains(source, "ViewBag.BestItems = data;");
        }

        private static string ReadSource(params string[] parts)
        {
            return File.ReadAllText(Path.Combine(GetRootPath(), Path.Combine(parts)));
        }

        private static string GetRootPath()
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);

            while (directory != null)
            {
                if (Directory.Exists(Path.Combine(directory.FullName, "Exer_1")) &&
                    Directory.Exists(Path.Combine(directory.FullName, "Exer_2")))
                {
                    return directory.FullName;
                }

                directory = directory.Parent;
            }

            throw new DirectoryNotFoundException("Cannot locate repository root containing Exer_1 and Exer_2.");
        }
    }
}
