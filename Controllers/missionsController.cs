using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectMosadApi.DAL;
using ProjectMosadApi.Models;
using ProjectMosadApi.Servicees;
using ProjectMosadApi.Servicees.chase;
using ProjectMosadApi.Utils;


namespace ProjectMosadApi.Controllers;

[ApiController]
[Route("[controller]")]
public class missionsController : ControllerBase

{
    private readonly D_DbContext _context;
    private readonly ILogger<missionsController> _logger;
    Chases chases;


    public missionsController(ILogger<missionsController> logger, D_DbContext context)
    {
        this._context = context;
        this._logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMissions()
    {

        var missions = await this._context.Missions.ToListAsync();
        return StatusCode(200, missions);
    }

    [HttpPut("{id}")]
    public IActionResult StartMission(int id)
    {
        M_Mission mission = _context.Missions.Find(id);
        if (mission == null)
        {
            return NotFound();
        }
        mission.StartTime = DateTime.Now;
        mission.Status = "Active";
        _context.Missions.Update(mission);
        _context.SaveChanges();
        return StatusCode(
            StatusCodes.Status200OK,
            new { message = "Mission started." });
        
    }

    [HttpPut("update")]
    public void UpdateMissions()
    {
        foreach (M_Mission mission in _context.Missions)
        {
            if (mission.Status == "Active")
            {
                if (mission.Agent.Loc_X == mission.Target.Loc_X && mission.Agent.Loc_Y == mission.Target.Loc_Y)
                {
                    mission.Status = "Finished";
                    mission.Target.Status = "Dead";
                    continue;
                }

                M_Agent agent = chases.OneStapChases(mission);
                if (mission.Agent.Loc_X == mission.Target.Loc_X && mission.Agent.Loc_Y == mission.Target.Loc_Y)
                {
                    mission.Status = "Finished";
                    mission.Target.Status = "Dead";
                    continue;
                }
                mission.Agent = agent;
                //עדכון הזמן שנותר לביצוע החיסול בעזרת הפונקציה של בדיקת המרחק
                mission.TimeMission = TestesSpaces.distance(mission.Agent, mission.Target);
                
            }
        }
        _context.SaveChanges();
    }

}