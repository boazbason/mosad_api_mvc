using System.ComponentModel.DataAnnotations;

namespace ProjectMosadApi.Models;

public class M_Mission
{
    [Key]
    public int Id { get; set; }
    public M_Agent Agent { get; set; }
    public M_Target Target { get; set; }
    public string Status { get; set; }
    public DateTime StartTime { get; set; }


    public int time_left()
    {
        //חישוב הזמן שנשאר
        return 0;
    }

    public int TimeOut()
    {
        return DateTime.Now.Minute - StartTime.Minute;
    }
}