using Microsoft.AspNetCore.Mvc;

namespace AnimeTracker.Areas.Anime.Controllers;

public class UserController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}