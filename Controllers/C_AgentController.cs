using Microsoft.AspNetCore.Mvc;
using ProjectMosadApi.Models;

namespace ProjectMosadApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class C_AgentController : ControllerBase
{
    //private readonly IronDomeContext _context;
    //private readonly ILogger<AttacksController> _logger;
    //public C_Agent(ILogger<C_Agent> logger, IronDomeContext context)
    //{
    //    
    //    this._context = context;
    //    this._logger = logger;
    //}
    
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public  IActionResult CreateAttack(M_Agent agent)
    {
        DBTemporary_list.AddAgent(agent);
        Console.WriteLine($"location {agent.Location[0]}");
        return StatusCode(
            StatusCodes.Status201Created,
            new {success = true, agent = agent}
        );
    }
    
    //[HttpGet]
    //public IActionResult GetAllAgent(){}
}