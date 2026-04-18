using System;

namespace DTO
{
    public class UserDTO
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Lock { get; set; }
    }

    public class ItemDTO
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string Size { get; set; }
    }

    public class AgentDTO
    {
        public int AgentID { get; set; }
        public string AgentName { get; set; }
        public string Address { get; set; }
    }

    public class OrderDTO
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public int AgentID { get; set; }
    }

    public class OrderDetailDTO
    {
        public int ID { get; set; }
        public int OrderID { get; set; }
        public int ItemID { get; set; }
        public int Quantity { get; set; }
        public decimal UnitAmount { get; set; }
    }
}
