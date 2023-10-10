using AnimeTracker.Data.Repositories.Interface;
using AnimeTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace AnimeTracker.Controllers;

public class AnimeController : Controller
{
    private readonly IAnimeRepository _animeRepository;

    // GET
    public AnimeController(IAnimeRepository animeRepository)
    {
        _animeRepository = animeRepository;
    }

    // public IActionResult Index()
    // {
    //     return View();
    // }
    // GET
    // public IActionResult Add()
    // {
    //     return View();
    // }
    // GET
    // [HttpPost]
    // public IActionResult Add(Anime anime)
    // {
    //     _animeRepository.Insert(anime);
    //     
    //     return RedirectToAction("Index");
    // }
}