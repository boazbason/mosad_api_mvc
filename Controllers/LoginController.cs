using Microsoft.AspNetCore.Mvc;
using ProjectMosadApi.DAL;
using ProjectMosadApi.Servicees;

namespace ProjectMosadApi.Controllers;



[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly D_DbContext _context;
    private readonly ILogger<targetsController> _logger;
    private string[] permissions = { "SimulationServer", "MVCserver" };
    

    public LoginController(ILogger<targetsController> logger, D_DbContext context)
    {
        this._context = context;
        this._logger = logger;
    }

    [HttpPost("login")]
    public IActionResult login([FromBody] Dictionary<string, string> parameters)
    {
        //בדיקה האם ישנה הרשאה למשתמש
        if (permissions.Contains(parameters["id"]))
        {
            //יצירת טוקן, שמירה, שליחה
            string new_goid = Guid.NewGuid().ToString();
            //בדיקה מי זה (שרת או סימולציה)
            if (parameters["id"] == "SimulationServer")
            {
                ManagerTokens.tokensSimulation.Add(new_goid);
            }
            else
            {
                ManagerTokens.tokensUsers.Add(new_goid);
            }
            return Ok(new { token = new_goid });
        }
        return Unauthorized();
    }
}        