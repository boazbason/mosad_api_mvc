using Microsoft.AspNetCore.Mvc;
using ProjectMosadApi.Models;

namespace MosadMVC.Controllers;

public class AgentsController : Controller
{
    private readonly HttpClient _httpClient;


    private readonly ILogger<HomeController> _logger;

    public AgentsController(ILogger<HomeController> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }
    // GET
    public IActionResult Index()
    {
        return View();
    }
    public async Task <IActionResult> AgentView()
    {
        //בקשת api לקבלת כל הסוכנים
        
        var res = await _httpClient.GetFromJsonAsync<IEnumerable<M_Agent>>("http://localhost:5266/agents");
        
        return View(res);
    }
}