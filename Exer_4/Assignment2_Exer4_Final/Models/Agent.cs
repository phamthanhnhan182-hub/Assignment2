using System.ComponentModel.DataAnnotations;
namespace Assignment2_Exer4_Final.Models;

public class Agent
{
    [Key] public int AgentId { get; set; }
    [Required] public string AgentName { get; set; }
}