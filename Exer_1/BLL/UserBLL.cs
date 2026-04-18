using DAL;

namespace BLL
{
    public class UserBLL
    {
        private readonly UserDAL _dal = new UserDAL();

        public bool Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return false;

            return _dal.CheckLogin(username.Trim(), password.Trim());
        }
    }
}
