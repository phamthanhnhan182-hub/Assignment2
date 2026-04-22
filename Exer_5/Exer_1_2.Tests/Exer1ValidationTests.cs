using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Exer_1_2.Tests
{
    [TestClass]
    public class Exer1ValidationTests
    {
        [DataTestMethod]
        [DataRow("Exer_1\\BLL\\UserBLL.cs")]
        [DataRow("Exer_1\\BLL\\ItemBLL.cs")]
        [DataRow("Exer_1\\BLL\\AgentBLL.cs")]
        [DataRow("Exer_1\\BLL\\OrderBLL.cs")]
        public void Exer1_Bll_SourceFiles_Exist(string relativePath)
        {
            var fullPath = Path.Combine(GetRootPath(), relativePath);
            Assert.IsTrue(File.Exists(fullPath), $"Missing file: {fullPath}");
        }

        [TestMethod]
        public void UserBLL_Contains_Whitespace_Validation_And_Trim()
        {
            var source = ReadSource("Exer_1", "BLL", "UserBLL.cs");

            StringAssert.Contains(source, "string.IsNullOrWhiteSpace(username)");
            StringAssert.Contains(source, "string.IsNullOrWhiteSpace(password)");
            StringAssert.Contains(source, "username.Trim()");
            StringAssert.Contains(source, "password.Trim()");
        }

        [TestMethod]
        public void ItemBLL_Contains_Insert_Validation()
        {
            var source = ReadSource("Exer_1", "BLL", "ItemBLL.cs");

            StringAssert.Contains(source, "string.IsNullOrWhiteSpace(itemName)");
            StringAssert.Contains(source, "string.IsNullOrWhiteSpace(size)");
            StringAssert.Contains(source, "_dal.Insert(itemName.Trim(), size.Trim())");
        }

        [TestMethod]
        public void ItemBLL_Contains_Update_Validation()
        {
            var source = ReadSource("Exer_1", "BLL", "ItemBLL.cs");

            StringAssert.Contains(source, "itemId <= 0");
            StringAssert.Contains(source, "_dal.Update(itemId, itemName.Trim(), size.Trim())");
        }

        [TestMethod]
        public void ItemBLL_Contains_Delete_Validation()
        {
            var source = ReadSource("Exer_1", "BLL", "ItemBLL.cs");

            StringAssert.Contains(source, "itemId > 0 && _dal.Delete(itemId) > 0");
        }

        [TestMethod]
        public void AgentBLL_Contains_Insert_Validation()
        {
            var source = ReadSource("Exer_1", "BLL", "AgentBLL.cs");

            StringAssert.Contains(source, "string.IsNullOrWhiteSpace(agentName)");
            StringAssert.Contains(source, "string.IsNullOrWhiteSpace(address)");
            StringAssert.Contains(source, "_dal.Insert(agentName.Trim(), address.Trim())");
        }

        [TestMethod]
        public void AgentBLL_Contains_Update_Validation()
        {
            var source = ReadSource("Exer_1", "BLL", "AgentBLL.cs");

            StringAssert.Contains(source, "agentId <= 0");
            StringAssert.Contains(source, "_dal.Update(agentId, agentName.Trim(), address.Trim())");
        }

        [TestMethod]
        public void AgentBLL_Contains_Delete_Validation()
        {
            var source = ReadSource("Exer_1", "BLL", "AgentBLL.cs");

            StringAssert.Contains(source, "agentId > 0 && _dal.Delete(agentId) > 0");
        }

        [TestMethod]
        public void OrderBLL_Contains_Agent_Validation()
        {
            var source = ReadSource("Exer_1", "BLL", "OrderBLL.cs");

            StringAssert.Contains(source, "agentId <= 0");
            StringAssert.Contains(source, "Please select an agent.");
        }

        [TestMethod]
        public void OrderBLL_Contains_Empty_Details_Validation()
        {
            var source = ReadSource("Exer_1", "BLL", "OrderBLL.cs");

            StringAssert.Contains(source, "details == null || details.Rows.Count == 0");
            StringAssert.Contains(source, "Order must have at least one detail line.");
        }

        [TestMethod]
        public void OrderBLL_Contains_Quantity_Validation()
        {
            var source = ReadSource("Exer_1", "BLL", "OrderBLL.cs");

            StringAssert.Contains(source, "Convert.ToInt32(row[\"Quantity\"])");
            StringAssert.Contains(source, "Quantity must be greater than 0.");
        }

        [TestMethod]
        public void OrderBLL_Contains_UnitAmount_Validation()
        {
            var source = ReadSource("Exer_1", "BLL", "OrderBLL.cs");

            StringAssert.Contains(source, "Convert.ToDecimal(row[\"UnitAmount\"])");
            StringAssert.Contains(source, "Unit amount must be greater than or equal to 0.");
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
