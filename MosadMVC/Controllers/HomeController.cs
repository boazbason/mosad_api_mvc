using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MosadMVC.Models;
using Newtonsoft.Json;
using ProjectMosadApi.Models;



namespace MosadMVC.Controllers;

public class HomeController : Controller
{
    private readonly HttpClient _httpClient;


    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task <IActionResult> main_page()
    {
        //קבלת שלושת הרשימות
        
        Dictionary<string, int> Data = new Dictionary<string, int>();
        var Missions = await _httpClient.GetFromJsonAsync<IEnumerable<M_Mission>>("http://localhost:5266/Missions");
        var agents = await _httpClient.GetFromJsonAsync<IEnumerable<M_Agent>>("http://localhost:5266/agents");
        var targets = await _httpClient.GetFromJsonAsync<IEnumerable<M_Target>>("http://localhost:5266/targets");
        Data["AgentsCount"] = 0;
        Data["ActiveAgents"] = 0;
        foreach (M_Agent agent in agents)
        {
            Data["AgentsCount"]++;
            if (agent.Status == "Active")
            {
                Data["ActiveAgents"]++;
            }
        }
        Data["TargetsCount"] = 0;
        Data["TargetsDeadcount"] = 0;
        foreach (M_Target target in targets)
        {
            Data["TargetsCount"]++;
            if (target.Status == "Dead")
            {
                Data["TargetsDeadcount"]++;
            }
        }
        Data["MissionCount"] = 0;
        Data["FinishedMission"] = 0;
        foreach (M_Mission mission in Missions)
        {
            Data["MissionCount"]++;
            if (mission.Status == "Finished")
            {
                Data["FinishedMission"]++;
            }
        }

        Data["AgentToTarget"] = Data["AgentsCount"] / Data["TargetsCount"];
        Data["ActiveagentToTarget"] = (Data["AgentsCount"] - Data["ActiveAgents"]) /
                                      (Data["TargetsCount"] - Data["TargetsDeadcount"]);

        return View(Data);
    }
    
    public async Task <IActionResult> ShowMissions()
    {
        //בקשת api לקבלת כל המשימות
        
        var res = await _httpClient.GetFromJsonAsync<IEnumerable<M_Mission>>("http://localhost:5266/Missions");
        //הכנסה לתוך כל משימה את הסוכן והמשימה שלה
        foreach (M_Mission mission in res)
        {
            var agent = await _httpClient.GetAsync($"http://localhost:5266/agents/GetAgent/{mission.AgentId}");
            var jsonResponse = await agent.Content.ReadAsStringAsync();
            mission.Agent = JsonConvert.DeserializeObject<M_Agent>(jsonResponse);
            var target = await _httpClient.GetAsync($"http://localhost:5266/agents/GetTarget/{mission.TargetId}");
            var jsonResponse2 = await agent.Content.ReadAsStringAsync();
            mission.Target = JsonConvert.DeserializeObject<M_Target>(jsonResponse2);

        }
        return View(res);
    }
    

    public async void StartMission(int id)
    {
        //שליחת בקשת API להפעלת המשימה שנשלחה
        string apiUrl = $"http://localhost:5266/Missions/{id}";
        await _httpClient.PutAsync(apiUrl, null);
        
    }
    
    
    
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}