using System.ComponentModel.DataAnnotations;

namespace ProjectMosadApi.Models;

public class M_Target
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Job { get; set; }
    public string? Picture { get; set; }
    public int Loc_X { get; set; } = 0;
    public int Loc_Y { get; set; } = 0;
    public string? Status { get; set; }
}