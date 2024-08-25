using Microsoft.EntityFrameworkCore;
using ProjectMosadApi.DAL;
using ProjectMosadApi.Models;

namespace ProjectMosadApi.Servicees;
//קלאס עם כל הלוגיקה של מיקומים
public class TestesSpaces
{
    private readonly D_DbContext _context;
    private readonly ILogger<TestesSpaces> _logger;
    
    public TestesSpaces(ILogger<TestesSpaces> logger, D_DbContext context)
    {
        this._context = context;
        this._logger = logger;
    }
    //פונקצייה לבדיקת מרחק בין שני אובייקטים
    public static double distance(M_Agent b, M_Target a)
    {
        return Math.Sqrt(Math.Pow(a.Loc_X - b.Loc_X, 2) + Math.Pow(b.Loc_Y - a.Loc_Y, 2));
    }
    
    
    //פונקציות להוספת משימה חדשה במקרה ויש סוכן או מטרה שקרובים
    public static List<M_Mission> Test_Targets_Around(M_Agent agent, DbSet<M_Target> targets)
    {
        List<M_Mission> missions = new List<M_Mission>();

        foreach (M_Target target in targets)
        {
            //שליחה לפונקצייה שבודקת מרחק
            if (distance(agent, target) < 200)
            {
                //אם המרחק קטן מ 200 יצירת משימה חדשה עם סטטוס אופציונלי
                M_Mission mission = new M_Mission();
                mission.Agent = agent;
                mission.Target = target;
                mission.Status = "Optional";
                mission.TimeMission = distance(agent, target) / 5.0;
                missions.Add(mission);
            }

        }
        return missions;

    }
    
    public static List<M_Mission> Test_Agents_Around(M_Target target, DbSet<M_Agent> agents)
    {
        List<M_Mission> missions = new List<M_Mission>();

        foreach (M_Agent agent in agents)
        {
            //שליחה לפונקצייה שבודקת מרחק
            if (distance(agent, target) < 200)
            {
                //אם המרחק קטן מ 200 יצירת משימה חדשה עם סטטוס אופציונלי
                M_Mission mission = new M_Mission();
                mission.Agent = agent;
                mission.Target = target;
                mission.Status = "Optional";
                mission.TimeMission = distance(agent, target) / 5.0;
                missions.Add(mission);

            }
        }
        return missions;
    }
}