using System.Data;
using DAL;

namespace BLL
{
    public class ReportBLL
    {
        private readonly ReportDAL _dal = new ReportDAL();

        public DataTable GetOrderReport(int orderId)
        {
            return _dal.GetOrderReport(orderId);
        }
    }
}
