using System.ComponentModel.DataAnnotations;

public class OrderDetail
{
    [Key] public int OrderDetailID { get; set; }
    public int OrderID { get; set; }
    public int ProductID { get; set; }
    public int Quantity { get; set; }
}