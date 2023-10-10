using System.Diagnostics;
using AnimeTracker.Data.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using AnimeTracker.Models;

namespace AnimeTracker.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IAnimeRepository _animeRepository;



    public HomeController(IAnimeRepository animeRepository)
    {
        _animeRepository = animeRepository;
    }

    public IActionResult Index()
    {
        List<Anime> animes = _animeRepository.List();
        return View(animes);
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