using System.ComponentModel.DataAnnotations;

namespace ProjectMosadApi.Models;

public class M_Agent
{
    [Key]
    public int Id { get; set; }
    public string Picture { get; set; }
    public string Name { get; set; }
    public int? Loc_X { get; set; }
    public int? Loc_Y { get; set; }
    public string? Status { get; set; }
}