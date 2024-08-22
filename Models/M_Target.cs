using System.ComponentModel.DataAnnotations;

namespace ProjectMosadApi.Models;

public class M_Target
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Job { get; set; }
    public int? Loc_X { get; set; }
    public int? Loc_Y { get; set; }
    public string? Status { get; set; }
}