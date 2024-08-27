using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectMosadApi.Models;
using ProjectMosadApi.DAL;
using ProjectMosadApi.Utils;
using ProjectMosadApi.Servicees;


namespace ProjectMosadApi.Controllers;


[ApiController]
[Route("[controller]")]
public class agentsController : ControllerBase
{
    private readonly D_DbContext _context;
    private readonly ILogger<agentsController> _logger;
    public agentsController(ILogger<agentsController> logger, D_DbContext context)
    {
        this._context = context;
        this._logger = logger;

    }
    
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public  IActionResult CreateAgent(M_Agent agent)
    {
        _context.Agents.Add(agent);
        _context.SaveChanges();
        
        return StatusCode(
            StatusCodes.Status201Created,
            new {success = true, agent = agent}
        );
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllAgent()
    {
        var agents = await this._context.Agents.ToListAsync();
        return Ok(agents);
    }

    [HttpGet("GetAgent/{id}")]
    public async Task<IActionResult> GetAgent(int id)
    {
        var agent = await this._context.Agents.FindAsync(id);
        if (agent == null)
        {
            return NotFound();
        }

        return Ok(agent);
    }

    [HttpPut ("{id}/move")]
    public async Task<IActionResult> MoveAgent(int id, [FromBody] Dictionary<string, string> directionDict)
    {
        //בדיקת הטוקן
        if (!ManagerTokens.tokensSimulation.Contains(directionDict["token"]))
        {
            return StatusCode(StatusCodes.Status401Unauthorized);
        }
        int status;
        var agent = await this._context.Agents.FindAsync(id);
        if (agent == null)
        {
            status = StatusCodes.Status404NotFound;
            return StatusCode(status, HttpUtils.Response(status, "agent not found"));
        }
        string move  = directionDict["direction"];
        if (move.Contains("n"))
        {
            if (agent.Loc_Y < 1)
            {
                return StatusCode(
                    StatusCodes.Status400BadRequest,
                    new { message = "X and Y should be in the range [1,1000]." });
            }
            agent.Loc_Y++;
            _context.Agents.Update(agent);
        }
        if (move.Contains("e"))
        {
            if (agent.Loc_X > 999)
            {
                return StatusCode(
                    StatusCodes.Status400BadRequest,
                    new { message = "X and Y should be in the range [1,1000]." });
            }
            agent.Loc_X++;
            _context.Agents.Update(agent);
        }
        if (move.Contains("s"))
        {
            if (agent.Loc_Y > 999)
            {
                return StatusCode(
                    StatusCodes.Status400BadRequest,
                    new { message = "X and Y should be in the range [1,1000]." });
            }
            agent.Loc_Y--;
            _context.Agents.Update(agent);
        }
        if (move.Contains("w"))
        {
            if (agent.Loc_X < 1)
            {
                return StatusCode(
                    StatusCodes.Status400BadRequest,
                    new { message = "X and Y should be in the range [1,1000]." });
            }
            agent.Loc_X--;
            _context.Agents.Update(agent);
        }
        
        //בדיקה האם יש מטרות באזור ואם כן אז ליצור משימה חדשה
        //פונקציית החזרת מערך של כל המשימות הרלוונטיות בסביבה
        List<M_Mission> missionsOptional = TestesSpaces.Test_Targets_Around(agent, _context.Targets);
        foreach (M_Mission mission in missionsOptional)
        {
            if (mission.Agent.Status == "Free" && mission.Target.Status == "Free" && !TestesSpaces.AlreadyFound(mission, _context.Missions))
            {
                _context.Missions.Add(mission);
                mission.Status = "Optional";
                _context.SaveChanges();

            }
        }
        _context.SaveChanges();

        return StatusCode(
            StatusCodes.Status200OK,
            new { message = "Agent Moved" });

    }

    [HttpPut("{id}/pin")]
    public async Task<IActionResult> SeetAgent(int id, [FromBody] Dictionary<string, string> directionDict)
    {
        //בדיקת הטוקן
        if (!ManagerTokens.tokensSimulation.Contains(directionDict["token"]))
        {
            return StatusCode(StatusCodes.Status401Unauthorized);
        }
        int status;
        var agent = await this._context.Agents.FindAsync(id);
        if (agent == null)
        {
            status = StatusCodes.Status404NotFound;
            return StatusCode(status, HttpUtils.Response(status, "agent not found"));
        }
        int x = Convert.ToInt32(directionDict["x"]);
        int y = Convert.ToInt32(directionDict["y"]);

        if (x > 1000 || x < 0 || y > 1000 || y < 0)
        {
            return StatusCode(
                StatusCodes.Status400BadRequest,
                new { message = "X and Y should be in the range [1,1000]." });
        }
        agent.Status = "Free";
        _context.Agents.Update(agent);
        
        //בדיקה האם יש מטרות באזור ואם כן אז ליצור משימה חדשה
        List<M_Mission> missionsOptional = TestesSpaces.Test_Targets_Around(agent, _context.Targets);
        foreach (M_Mission mission in missionsOptional)
        {
            if (mission.Agent.Status == "Free" && mission.Target.Status == "Free" && !TestesSpaces.AlreadyFound(mission, _context.Missions))
            {
                _context.Missions.Add(mission);
                mission.Status = "Optional";
                _context.SaveChanges();

            }
        }
        _context.SaveChanges();

        return StatusCode(
            StatusCodes.Status200OK,
            new { message = "Agent Location seated" });
    }
}