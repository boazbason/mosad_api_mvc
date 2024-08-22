using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectMosadApi.Models;
using ProjectMosadApi.DAL;
using ProjectMosadApi.Utils;



namespace ProjectMosadApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class C_TargetController : ControllerBase
{
    private readonly D_DbContext _context;
    private readonly ILogger<C_TargetController> _logger;
    
    public C_TargetController(ILogger<C_TargetController> logger, D_DbContext context)
    {
        this._context = context;
        this._logger = logger;
    }
    
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public  IActionResult CreateTarget(M_Target target)
    {
        Console.WriteLine("Target Creating");
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
        int status = StatusCodes.Status200OK;
        var Targets = await this._context.Targets.ToListAsync();
        return StatusCode(
            status,
            HttpUtils.Response(status, new {Targets = Targets})
        );
    }
    [HttpPut("{id}/pin")]
    public async Task<IActionResult> SeetTarget(int id, [FromBody] Dictionary<string, string> directionDict)
    {
        int status;
        var Target = await this._context.Targets.FindAsync(id);
        if (Target == null)
        {
            status = StatusCodes.Status404NotFound;
            return StatusCode(status, HttpUtils.Response(status, "Target not found"));
        }
        Target.Loc_X = Convert.ToInt32(directionDict["x"]) ;
        Target.Loc_Y = Convert.ToInt32(directionDict["y"]) ;
        _context.Targets.Update(Target);
        _context.SaveChanges();
        return StatusCode(
            StatusCodes.Status200OK,
            new { message = "Target Location seated" });
    }
    
    [HttpPut ("{id}")]
    public async Task<IActionResult> MoveAgent(int id, [FromBody] Dictionary<string, string> directionDict)
    {
        int status;
        var target = await this._context.Agents.FindAsync(id);
        if (target == null)
        {
            status = StatusCodes.Status404NotFound;
            return StatusCode(status, HttpUtils.Response(status, "target not found"));
        }
        string move  = directionDict["direction"];
        if (move.Contains("n"))
        {
            target.Loc_Y++;
            _context.Agents.Update(target);
            _context.SaveChanges();
        }
        if (move.Contains("e"))
        {
            target.Loc_X++;
            _context.Agents.Update(target);
            _context.SaveChanges();
        }
        if (move.Contains("s"))
        {
            target.Loc_Y--;
            _context.Agents.Update(target);
            _context.SaveChanges();
        }
        if (move.Contains("w"))
        {
            target.Loc_X--;
            _context.Agents.Update(target);
            _context.SaveChanges();
        }

        return StatusCode(
            StatusCodes.Status200OK,
            new { message = "Target Moved" });

    }

}