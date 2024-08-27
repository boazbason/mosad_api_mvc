using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
    
    public async Task <IActionResult> AgentView()
    {
        //בקשת api לקבלת כל הסוכנים
        
        var res = await _httpClient.GetFromJsonAsync<IEnumerable<M_Agent>>("http://localhost:5266/agents");
        
        return View(res);
    }
    
    //בקשה לקבלת סוכן ספיציפי
    public async Task<IActionResult> GetAgent(int id)
    {
        var agent = await _httpClient.GetAsync($"http://localhost:5266/agents/GetAgent/{id}");
        var jsonResponse = await agent.Content.ReadAsStringAsync();
        M_Agent Agent = JsonConvert.DeserializeObject<M_Agent>(jsonResponse);
        return View(Agent);
    }
}