using Microsoft.AspNetCore.Mvc;
using ProjectMosadApi.Models;

namespace MosadMVC.Controllers;

public class TargetsController : Controller
{
    private readonly HttpClient _httpClient;


    private readonly ILogger<HomeController> _logger;

    public TargetsController(ILogger<HomeController> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }
    public async Task <IActionResult> TaretsView()
    {
        //בקשת api לקבלת כל המטרות
        
        var res = await _httpClient.GetFromJsonAsync<IEnumerable<M_Target>>("http://localhost:5266/targets");
        
        return View(res);
    }
}