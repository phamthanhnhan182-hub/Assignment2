using System.ComponentModel.DataAnnotations;
namespace Assignment2_Exer4_Final.Models;

public class Product
{
    [Key] public int ProductId { get; set; }
    [Required] public string Name { get; set; }
    public decimal Price { get; set; }
}