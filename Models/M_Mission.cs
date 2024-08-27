using System.ComponentModel.DataAnnotations;

namespace ProjectMosadApi.Models;

public class M_Mission
{
    [Key]
    public int Id { get; set; }
    public M_Agent Agent { get; set; }
    public int AgentId { get; set; }
    public M_Target Target { get; set; }
    public int TargetId { get; set; }
    public string Status { get; set; }
    public double? ExecutionTime { get; set; }
    public double? TimeMission { get; set; }

    
}