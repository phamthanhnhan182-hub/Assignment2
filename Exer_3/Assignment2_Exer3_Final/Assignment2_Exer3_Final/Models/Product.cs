using System.ComponentModel.DataAnnotations;
namespace Assignment2_Exer3_Final.Models;

public class Product
{
    [Key] public int ProductID { get; set; }
    [Required] public string ProductName { get; set; }
    public decimal Price { get; set; }
}