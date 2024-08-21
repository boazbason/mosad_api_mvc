using System.ComponentModel.DataAnnotations;

namespace ProjectMosadApi.Models;

public class M_Target
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Job { get; set; }
    public int[] Location = { 0, 0 };
    public string Status { get; set; }
}