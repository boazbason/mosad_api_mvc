using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MosadMVC.Models;
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

    public IActionResult Index()
    {
        return View();
    }
    
    public async Task <IActionResult> ShowMissions()
    {
        //בקשת api לקבלת כל המשימות
        
        var res = await _httpClient.GetFromJsonAsync<IEnumerable<M_Mission>>("http://localhost:5266/Missions");
        
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