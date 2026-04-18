using System.Data;
using DAL;

namespace BLL
{
    public class ItemBLL
    {
        private readonly ItemDAL _dal = new ItemDAL();

        public DataTable GetAll() => _dal.GetAll();
        public DataTable Search(string keyword) => _dal.Search(keyword ?? string.Empty);

        public bool Insert(string itemName, string size)
        {
            if (string.IsNullOrWhiteSpace(itemName) || string.IsNullOrWhiteSpace(size))
                return false;
            return _dal.Insert(itemName.Trim(), size.Trim()) > 0;
        }

        public bool Update(int itemId, string itemName, string size)
        {
            if (itemId <= 0 || string.IsNullOrWhiteSpace(itemName) || string.IsNullOrWhiteSpace(size))
                return false;
            return _dal.Update(itemId, itemName.Trim(), size.Trim()) > 0;
        }

        public bool Delete(int itemId)
        {
            return itemId > 0 && _dal.Delete(itemId) > 0;
        }
    }
}
