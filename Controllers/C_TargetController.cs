using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectMosadApi.Models;
using ProjectMosadApi.DAL;
using ProjectMosadApi.Utils;
using ProjectMosadApi.Servicees;



namespace ProjectMosadApi.Controllers;


[ApiController]
[Route("[controller]")]
public class targetsController : ControllerBase
{
    private readonly D_DbContext _context;
    private readonly ILogger<targetsController> _logger;
    
    public targetsController(ILogger<targetsController> logger, D_DbContext context)
    {
        this._context = context;
        this._logger = logger;

    }
    
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public  IActionResult CreateTarget(M_Target target)
    {
        //Console.WriteLine("Target Creating");
        _context.Targets.Add(target);
        _context.SaveChanges();
        return StatusCode(
            StatusCodes.Status201Created,
            new {success = true, target = target}
        );
    }
    [HttpGet]
    public async Task<IActionResult> GetAllTarget()
    {
        var Targets = await this._context.Targets.ToListAsync();
        return Ok(Targets);
    }
    [HttpPut("{id}/pin")]
    public async Task<IActionResult> SeetTarget(int id, [FromBody] Dictionary<string, string> directionDict)
    {
        //בדיקת הטוקן
        if (!ManagerTokens.tokensSimulation.Contains(directionDict["token"]))
        {
            return StatusCode(StatusCodes.Status401Unauthorized);
        }
        int status;
        var Target = await this._context.Targets.FindAsync(id);
        if (Target == null)
        {
            status = StatusCodes.Status404NotFound;
            return StatusCode(status, HttpUtils.Response(status, "Target not found"));
        }

        int x = Convert.ToInt32(directionDict["x"]);
        int y = Convert.ToInt32(directionDict["y"]);

        if (x > 1000 || x < 0 || y > 1000 || y < 0)
        {
            return StatusCode(
                StatusCodes.Status400BadRequest,
                new { message = "X and Y should be in the range [1,1000]." });
        }
        
        Target.Loc_X = x ;
        Target.Loc_Y = y;
        Target.Status = "Free";
        _context.Targets.Update(Target);

        //בדיקה האם יש סוכנים באזור ואם כן אז ליצור משימה חדשה
        List<M_Mission> agentsOptional = TestesSpaces.Test_Agents_Around(Target, _context.Agents);
        foreach (M_Mission mission in agentsOptional)
        {
            if (mission.Agent.Status == "Free" && mission.Target.Status == "Free" &&
                !TestesSpaces.AlreadyFound(mission, _context.Missions))
            {
                _context.Missions.Add(mission);
                mission.Status = "Optional";

            }
        }
        _context.SaveChanges();
        
        return StatusCode(
            StatusCodes.Status200OK,
            new { message = "Target Location seated" });
    }
    
    [HttpPut ("{id}/move")]
    public async Task<IActionResult> MoveTarget(int id, [FromBody] Dictionary<string, string> directionDict)
    {
        //בדיקת הטוקן
        if (!ManagerTokens.tokensSimulation.Contains(directionDict["token"]))
        {
            return StatusCode(StatusCodes.Status401Unauthorized);
        }
        int status;
        var target = await this._context.Targets.FindAsync(id);
        if (target == null)
        {
            status = StatusCodes.Status404NotFound;
            return StatusCode(status, HttpUtils.Response(status, "target not found"));
        }
        string move  = directionDict["direction"];
        if (move.Contains("n"))
        {
            target.Loc_Y++;
            _context.Targets.Update(target);
        }
        if (move.Contains("e"))
        {
            target.Loc_X++;
            _context.Targets.Update(target);
        }
        if (move.Contains("s"))
        {
            target.Loc_Y--;
            _context.Targets.Update(target);
        }
        if (move.Contains("w"))
        {
            target.Loc_X--;
            _context.Targets.Update(target);
        }
        
        //בדיקה האם יש סוכנים באזור ואם כן אז ליצור משימה חדשה
        List<M_Mission> agentsOptional = TestesSpaces.Test_Agents_Around(target, _context.Agents);
        foreach (M_Mission mission in agentsOptional)
        {
            if (mission.Agent.Status == "Free" && mission.Target.Status == "Free" &&
                !TestesSpaces.AlreadyFound(mission, _context.Missions))
            {
                _context.Missions.Add(mission);
                mission.Status = "Optional";

            }
        }
        _context.SaveChanges();

        return StatusCode(
            StatusCodes.Status200OK,
            new { message = "Target Moved" });

    }
    [HttpGet("GetTarget/{id}")]
    public async Task<IActionResult> GetTarget(int id)
    {
        var target = await this._context.Targets.FindAsync(id);
        if (target == null)
        {
            return NotFound();
        }

        return Ok(target);
    }

}