using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class OrderDAL
    {
        private readonly string _connStr = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
        private readonly DbHelper _db = new DbHelper();

        public DataTable GetAgents()
        {
            return _db.ExecuteQuery("SELECT AgentID, AgentName FROM Agent ORDER BY AgentName");
        }

        public DataTable GetItems()
        {
            return _db.ExecuteQuery("SELECT ItemID, ItemName FROM Item ORDER BY ItemName");
        }

        public int SaveOrder(DateTime orderDate, int agentId, DataTable details)
        {
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        const string sqlOrder = @"
                            INSERT INTO [Order](OrderDate, AgentID)
                            VALUES(@OrderDate, @AgentID);
                            SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        int orderId;
                        using (var cmdOrder = new SqlCommand(sqlOrder, conn, trans))
                        {
                            cmdOrder.Parameters.AddWithValue("@OrderDate", orderDate);
                            cmdOrder.Parameters.AddWithValue("@AgentID", agentId);
                            orderId = (int)cmdOrder.ExecuteScalar();
                        }

                        foreach (DataRow row in details.Rows)
                        {
                            const string sqlDetail = @"
                                INSERT INTO OrderDetail(OrderID, ItemID, Quantity, UnitAmount)
                                VALUES(@OrderID, @ItemID, @Quantity, @UnitAmount)";

                            using (var cmdDetail = new SqlCommand(sqlDetail, conn, trans))
                            {
                                cmdDetail.Parameters.AddWithValue("@OrderID", orderId);
                                cmdDetail.Parameters.AddWithValue("@ItemID", Convert.ToInt32(row["ItemID"]));
                                cmdDetail.Parameters.AddWithValue("@Quantity", Convert.ToInt32(row["Quantity"]));
                                cmdDetail.Parameters.AddWithValue("@UnitAmount", Convert.ToDecimal(row["UnitAmount"]));
                                cmdDetail.ExecuteNonQuery();
                            }
                        }

                        trans.Commit();
                        return orderId;
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
