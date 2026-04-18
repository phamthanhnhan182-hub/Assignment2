using System.Data;
using DAL;

namespace BLL
{
    public class AgentBLL
    {
        private readonly AgentDAL _dal = new AgentDAL();

        public DataTable GetAll() => _dal.GetAll();
        public DataTable Search(string keyword) => _dal.Search(keyword ?? string.Empty);

        public bool Insert(string agentName, string address)
        {
            if (string.IsNullOrWhiteSpace(agentName) || string.IsNullOrWhiteSpace(address))
                return false;
            return _dal.Insert(agentName.Trim(), address.Trim()) > 0;
        }

        public bool Update(int agentId, string agentName, string address)
        {
            if (agentId <= 0 || string.IsNullOrWhiteSpace(agentName) || string.IsNullOrWhiteSpace(address))
                return false;
            return _dal.Update(agentId, agentName.Trim(), address.Trim()) > 0;
        }

        public bool Delete(int agentId)
        {
            return agentId > 0 && _dal.Delete(agentId) > 0;
        }
    }
}
