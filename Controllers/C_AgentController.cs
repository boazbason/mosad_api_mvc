using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectMosadApi.Models;
using ProjectMosadApi.DAL;
using ProjectMosadApi.Utils;



namespace ProjectMosadApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class C_AgentController : ControllerBase
{
    private readonly D_DbContext _context;
    private readonly ILogger<C_AgentController> _logger;
    
    public C_AgentController(ILogger<C_AgentController> logger, D_DbContext context)
    {
        this._context = context;
        this._logger = logger;
    }
    
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public  IActionResult CreateAttack(M_Agent agent)
    {
        Console.WriteLine("Agent Creating");
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
        int status = StatusCodes.Status200OK;
        var attacks = await this._context.Agents.ToListAsync();
        return StatusCode(
            status,
            HttpUtils.Response(status, new {attacks = attacks})
        );
    }

    [HttpGet("GetAgent/{id}")]
    public async Task<IActionResult> GetAgent(int id)
    {
        int status;
        var agent = await this._context.Agents.FindAsync(id);
        if (agent == null)
        {
            status = StatusCodes.Status404NotFound;
            return StatusCode(status, HttpUtils.Response(status, "agent not found"));
        }

        status = StatusCodes.Status200OK;
        return StatusCode(status, HttpUtils.Response(status, new { agent = agent }));
        
    }

    [HttpPut ("{id}")]
    public async Task<IActionResult> MoveAgent(int id, [FromBody] Dictionary<string, string> directionDict)
    {
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
            agent.Loc_Y++;
            _context.Agents.Update(agent);
            _context.SaveChanges();
        }
        if (move.Contains("e"))
        {
            agent.Loc_X++;
            _context.Agents.Update(agent);
            _context.SaveChanges();
        }
        if (move.Contains("s"))
        {
            agent.Loc_Y--;
            _context.Agents.Update(agent);
            _context.SaveChanges();
        }
        if (move.Contains("w"))
        {
            agent.Loc_X--;
            _context.Agents.Update(agent);
            _context.SaveChanges();
        }

        return StatusCode(
            StatusCodes.Status200OK,
            new { message = "Agent Moved" });

    }

    [HttpPut("{id}/pin")]
    public async Task<IActionResult> SeetAgent(int id, [FromBody] Dictionary<string, string> directionDict)
    {
        int status;
        var agent = await this._context.Agents.FindAsync(id);
        if (agent == null)
        {
            status = StatusCodes.Status404NotFound;
            return StatusCode(status, HttpUtils.Response(status, "agent not found"));
        }
        agent.Loc_X = Convert.ToInt32(directionDict["x"]) ;
        agent.Loc_Y = Convert.ToInt32(directionDict["y"]) ;
        _context.Agents.Update(agent);
        _context.SaveChanges();
        return StatusCode(
            StatusCodes.Status200OK,
            new { message = "Agent Location seated" });
    }
}