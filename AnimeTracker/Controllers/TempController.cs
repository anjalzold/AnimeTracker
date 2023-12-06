using Microsoft.AspNetCore.Mvc;

namespace AnimeTracker.Controllers;

public class TempController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}