using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class ItemDAL
    {
        private readonly DbHelper _db = new DbHelper();

        public DataTable GetAll()
        {
            return _db.ExecuteQuery("SELECT * FROM Item ORDER BY ItemID DESC");
        }

        public DataTable Search(string keyword)
        {
            return _db.ExecuteQuery("SELECT * FROM Item WHERE ItemName LIKE @Keyword ORDER BY ItemID DESC",
                new SqlParameter("@Keyword", "%" + keyword + "%"));
        }

        public int Insert(string itemName, string size)
        {
            return _db.ExecuteNonQuery("INSERT INTO Item(ItemName, Size) VALUES(@ItemName, @Size)",
                new SqlParameter("@ItemName", itemName),
                new SqlParameter("@Size", size));
        }

        public int Update(int itemId, string itemName, string size)
        {
            return _db.ExecuteNonQuery("UPDATE Item SET ItemName=@ItemName, Size=@Size WHERE ItemID=@ItemID",
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@ItemName", itemName),
                new SqlParameter("@Size", size));
        }

        public int Delete(int itemId)
        {
            return _db.ExecuteNonQuery("DELETE FROM Item WHERE ItemID=@ItemID",
                new SqlParameter("@ItemID", itemId));
        }
    }
}
