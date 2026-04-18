using System.ComponentModel.DataAnnotations;
namespace Assignment2_Exer3_Final.Models;

public class Agent
{
    [Key] public int AgentID { get; set; }
    [Required] public string AgentName { get; set; }
    public string Address { get; set; }
}