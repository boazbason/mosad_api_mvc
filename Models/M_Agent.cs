using System.ComponentModel.DataAnnotations;

namespace ProjectMosadApi.Models;

public class M_Agent
{
    [Key]
    public int Id { get; set; }
    public string Picture { get; set; }
    public string Name { get; set; }
    public int[]? Location = {0, 0};
    public string? Status { get; set; }
}