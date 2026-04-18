using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class AgentDAL
    {
        private readonly DbHelper _db = new DbHelper();

        public DataTable GetAll()
        {
            return _db.ExecuteQuery("SELECT * FROM Agent ORDER BY AgentID DESC");
        }

        public DataTable Search(string keyword)
        {
            return _db.ExecuteQuery("SELECT * FROM Agent WHERE AgentName LIKE @Keyword ORDER BY AgentID DESC",
                new SqlParameter("@Keyword", "%" + keyword + "%"));
        }

        public int Insert(string agentName, string address)
        {
            return _db.ExecuteNonQuery("INSERT INTO Agent(AgentName, Address) VALUES(@AgentName, @Address)",
                new SqlParameter("@AgentName", agentName),
                new SqlParameter("@Address", address));
        }

        public int Update(int agentId, string agentName, string address)
        {
            return _db.ExecuteNonQuery("UPDATE Agent SET AgentName=@AgentName, Address=@Address WHERE AgentID=@AgentID",
                new SqlParameter("@AgentID", agentId),
                new SqlParameter("@AgentName", agentName),
                new SqlParameter("@Address", address));
        }

        public int Delete(int agentId)
        {
            return _db.ExecuteNonQuery("DELETE FROM Agent WHERE AgentID=@AgentID",
                new SqlParameter("@AgentID", agentId));
        }
    }
}
