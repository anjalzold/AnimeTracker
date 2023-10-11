using System.Diagnostics;
using AnimeTracker.Areas.Anime.Models;
using AnimeTracker.Data.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using AnimeTracker.Models;

namespace AnimeTracker.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IAnimeRepository _animeRepository;
    private readonly ICategoryRepository _categoryRepository;




    public HomeController(IAnimeRepository animeRepository,ICategoryRepository categoryRepository)
    {
        _animeRepository = animeRepository;
        _categoryRepository = categoryRepository;

        
    }

 

    public IActionResult Index()
    {
        try
        {
            var animes = _animeRepository.List();

            var category = _categoryRepository.List();

            var animeList = new List<Animevm>();

            foreach (var anime in animes)
            {

                var animee = new Animevm
                {
                    AnimeName = anime.Name,
                    ImageFileName = anime.ImageFileName
                };
                animeList.Add(animee);

            }
        
            return View(animeList);
        }
        catch (Exception e)
        {
            return RedirectToAction("Index");
        }
       
    }

    public IActionResult Privacy()
    {
        return View();
    }
    //
    // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    // public IActionResult Error()
    // {
    //     return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    // }
    
   

}