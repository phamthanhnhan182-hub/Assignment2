using System.Data;
using DAL;

namespace BLL
{
    public class StatisticBLL
    {
        private readonly StatisticDAL _dal = new StatisticDAL();

        public DataTable GetBestItems() => _dal.GetBestItems();
        public DataTable GetItemsPurchasedByCustomers() => _dal.GetItemsPurchasedByCustomers();
        public DataTable GetCustomerPurchaseSummary() => _dal.GetCustomerPurchaseSummary();
    }
}
