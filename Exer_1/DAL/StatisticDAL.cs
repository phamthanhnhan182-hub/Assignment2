using System.Data;

namespace DAL
{
    public class StatisticDAL
    {
        private readonly DbHelper _db = new DbHelper();

        public DataTable GetBestItems()
        {
            return _db.ExecuteQuery(@"
                SELECT i.ItemID, i.ItemName, SUM(od.Quantity) AS TotalSold
                FROM OrderDetail od
                JOIN Item i ON od.ItemID = i.ItemID
                GROUP BY i.ItemID, i.ItemName
                ORDER BY TotalSold DESC");
        }

        public DataTable GetItemsPurchasedByCustomers()
        {
            return _db.ExecuteQuery(@"
                SELECT a.AgentName, i.ItemName, od.Quantity, od.UnitAmount, o.OrderDate
                FROM [Order] o
                JOIN Agent a ON o.AgentID = a.AgentID
                JOIN OrderDetail od ON o.OrderID = od.OrderID
                JOIN Item i ON od.ItemID = i.ItemID
                ORDER BY a.AgentName, o.OrderDate");
        }

        public DataTable GetCustomerPurchaseSummary()
        {
            return _db.ExecuteQuery(@"
                SELECT a.AgentName, COUNT(DISTINCT o.OrderID) AS TotalOrders, SUM(od.Quantity) AS TotalQuantity
                FROM Agent a
                JOIN [Order] o ON a.AgentID = o.AgentID
                JOIN OrderDetail od ON o.OrderID = od.OrderID
                GROUP BY a.AgentName
                ORDER BY TotalQuantity DESC");
        }
    }
}
