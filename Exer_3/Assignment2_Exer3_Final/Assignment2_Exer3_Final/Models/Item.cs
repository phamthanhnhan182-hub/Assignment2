using System.ComponentModel.DataAnnotations;
namespace Assignment2_Exer3_Final.Models;

public class Item
{
    [Key]
    public int ItemID { get; set; }
    public string ItemName { get; set; }
    public string Size { get; set; }
}