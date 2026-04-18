using System;
using System.Data;
using DAL;

namespace BLL
{
    public class OrderBLL
    {
        private readonly OrderDAL _dal = new OrderDAL();

        public DataTable GetAgents() => _dal.GetAgents();
        public DataTable GetItems() => _dal.GetItems();

        public int SaveOrder(DateTime orderDate, int agentId, DataTable details)
        {
            if (agentId <= 0)
                throw new Exception("Please select an agent.");

            if (details == null || details.Rows.Count == 0)
                throw new Exception("Order must have at least one detail line.");

            foreach (DataRow row in details.Rows)
            {
                if (Convert.ToInt32(row["Quantity"]) <= 0)
                    throw new Exception("Quantity must be greater than 0.");
                if (Convert.ToDecimal(row["UnitAmount"]) < 0)
                    throw new Exception("Unit amount must be greater than or equal to 0.");
            }

            return _dal.SaveOrder(orderDate, agentId, details);
        }
    }
}
