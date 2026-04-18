using System.ComponentModel.DataAnnotations;

public class Order
{
    [Key] public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public string CustomerName { get; set; }
}