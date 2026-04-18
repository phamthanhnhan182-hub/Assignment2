using System.Data.SqlClient;

namespace DAL
{
    public class UserDAL
    {
        private readonly DbHelper _db = new DbHelper();

        public bool CheckLogin(string username, string password)
        {
            const string sql = @"SELECT COUNT(*) FROM Users 
                                 WHERE UserName = @UserName 
                                   AND [Password] = @Password 
                                   AND [Lock] = 0";

            int count = (int)_db.ExecuteScalar(sql,
                new SqlParameter("@UserName", username),
                new SqlParameter("@Password", password));

            return count > 0;
        }
    }
}
