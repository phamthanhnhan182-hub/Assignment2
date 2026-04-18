using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class ReportDAL
    {
        private readonly DbHelper _db = new DbHelper();

        public DataTable GetOrderReport(int orderId)
        {
            return _db.ExecuteQuery(@"
                SELECT o.OrderID, o.OrderDate, a.AgentName, a.Address,
                       i.ItemName, od.Quantity, od.UnitAmount,
                       od.Quantity * od.UnitAmount AS LineTotal
                FROM [Order] o
                JOIN Agent a ON o.AgentID = a.AgentID
                JOIN OrderDetail od ON o.OrderID = od.OrderID
                JOIN Item i ON od.ItemID = i.ItemID
                WHERE o.OrderID = @OrderID
                ORDER BY od.ID",
                new SqlParameter("@OrderID", orderId));
        }
    }
}
