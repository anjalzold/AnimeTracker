using Microsoft.AspNetCore.Mvc;

namespace AnimeTracker.Areas.Anime.Controllers;

public class CategoryController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}