using ProjectMosadApi.DAL;
using ProjectMosadApi.Models;

namespace ProjectMosadApi.Servicees.chase;

public class Chases
{
    private readonly D_DbContext _context;
    private readonly ILogger<Chases> _logger;
    public Chases(ILogger<Chases> logger, D_DbContext context)
    {
        this._context = context;
        this._logger = logger;
        TestesSpaces _tesSpaces;

    }
    
    
    
    public M_Agent OneStapChases(M_Mission mission)
    {
        M_Agent agent = mission.Agent;
        M_Target target = mission.Target;
        

        if (agent.Loc_X > target.Loc_X)
        {
            agent.Loc_X--;
        }

        if (agent.Loc_X < target.Loc_X)
        {
            agent.Loc_X++;
        }

        if (agent.Loc_Y > target.Loc_Y)
        {
            agent.Loc_Y--;
        }

        if (agent.Loc_Y < target.Loc_Y)
        {
            agent.Loc_Y++;
        }
        return agent;
    }
}