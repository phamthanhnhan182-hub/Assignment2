using System.ComponentModel.DataAnnotations;

public class Order
{
    [Key] public int OrderID { get; set; }
    public DateTime OrderDate { get; set; }
    public int AgentID { get; set; } // Khóa ngoại tới Agent
}