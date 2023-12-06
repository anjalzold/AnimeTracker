using AnimeTracker.Areas.Anime.Models;
using AnimeTracker.Data.Repositories.Interface;
using AnimeTracker.Enums;
using AnimeTracker.Models;
using AnimeTracker.ViewModel;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace AnimeTracker.Controllers;

public class TempController : Controller
{
    public TempController(ILogger<TempController> logger, IAnimeRepository animeRepository,
        ICategoryRepository categoryRepository, IUsers users, INotyfService notifyService, IUserAnimeList userAnimeList,
        IUserAnimeStatus userAnimeStatus)
    {
        _logger = logger;
        _animeRepository = animeRepository;
        _categoryRepository = categoryRepository;
        _users = users;
        _notifyService = notifyService;
        _userAnimeList = userAnimeList;
        _userAnimeStatus = userAnimeStatus;
    }

    public INotyfService _notifyService { get; }

    private readonly ILogger<TempController> _logger;
    private readonly IAnimeRepository _animeRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUsers _users;
    private readonly IUserAnimeList _userAnimeList;
    private readonly IUserAnimeStatus _userAnimeStatus;


    public IActionResult UserProfile()
    {
        var username = HttpContext.Session.GetString("UserName");
        var userId = _users.List()
            .Where(a => a.UserName == username)
            .Select(a => a.UserId).FirstOrDefault();

        if (userId == Guid.Empty)
        {
            // Handle the case where the user was not found
            return View(new List<Animevm>()); // You can return an empty list or handle it differently
        }
        
        var userAnimeStatus = _userAnimeStatus.List()
            .Where(status => status.UserId == userId)
            .ToList();
        
        var animeList = new List<Animevm>();

        foreach (var status in userAnimeStatus)
        {
            // For each UserAnimeStatus, you can retrieve the associated anime
            var anime = _animeRepository.Find(status.AnimeId);
            if (anime != null)
            {

                var userAnimeList = status.UserAnimeList.UserAnimeStatus.FirstOrDefault(a => a.UserId == userId);
                

                var animeVm = new Animevm
                {
                    AnimeId = anime.AnimeId,
                    AnimeName = anime.Name,
                    Episode = userAnimeList!=null ? userAnimeList.UserAnimeList.Episode :0,
                    
                    // Add other properties you want to display
                };
                animeList.Add(animeVm);
            }
        }


        return View(animeList);
    }

    [HttpGet]
    public IActionResult TempForm(Guid id)
    {
        var username = HttpContext.Session.GetString("UserName");
        var userId = _users.List()
            .Where(a => a.UserName == username)
            .Select(a => a.UserId).FirstOrDefault();

        _logger.LogInformation("User ID: {username}",username);

        var animes = _animeRepository.Find(id);
        var animename = animes.Name;
        var images = animes.ImageFileName;
        
        var episode = 0; // Default to 0 if user's progress is not found
        // Fetch the user's progress for this anime if available
        var userAnimeStatus = _userAnimeStatus.List()
            .FirstOrDefault(status => status.UserId == userId && status.AnimeId == id);

        if (userAnimeStatus != null)
        {
            episode = userAnimeStatus.UserAnimeList.Episode;
        }


        var anime = new UserAnimeVm();
        anime.UserId = userId;
        anime.AnimeId = id;
        anime.AnimeName = animename;
        anime.Image = images;
        anime.Episode = episode;
        anime.Status = userAnimeStatus.UserAnimeList.Status;
        return View(anime);
    }

    [HttpPost]
    public IActionResult TempForm(UserAnimeVm vm)
    {

        if (vm.UserId != Guid.Empty) // Check if a valid user was found
        {

            var existingUserAnimeList = _userAnimeList.List()
                .FirstOrDefault(u => u.UserAnimeStatus.Any(ui => ui.UserId == vm.UserId && ui.AnimeId == vm.AnimeId));

            if (existingUserAnimeList != null)
            {
                // If the anime is already in the user's list, update the status and episode
                existingUserAnimeList.Status = vm.Status;
                existingUserAnimeList.Episode = vm.Episode;

                try
                {
                    _userAnimeList.Update(existingUserAnimeList);
                    return RedirectToAction("UserProfile");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while saving the data.");
                    Console.WriteLine(e);
                    return View(vm);
                }
            }
            else
            {
                var newUserAnimeList = new UserAnimeList
                {
                    Status = vm.Status,
                    Episode = vm.Episode,
                    UserAnimeStatus = new List<UserAnimeStatus>
                    {
                        new UserAnimeStatus
                        {
                            UserId = vm.UserId,
                            AnimeId = vm.AnimeId
                        }
                    }
                };

                try
                {
                    _userAnimeList.Insert(newUserAnimeList);
                    return RedirectToAction("UserProfile");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while saving the data.");
                    Console.WriteLine(e);
                    return View(vm);
                }
            }
            }

        //     var epi = _animeRepository.Find(vm.AnimeId);
        //
        //     var episode = epi.Episode;
        //     if (vm.Status == Status.Completed && vm.Episode != episode)
        //     {
        //         ModelState.AddModelError("Episode", "The episode number must be equal to the total number of episodes to mark it as 'Completed'.");
        //     }
        //     else
        //     {
        //         var userAnimeListt = new UserAnimeList
        //         {
        //             Status = vm.Status,
        //             Episode = vm.Episode,
        //             UserAnimeStatus = new List<UserAnimeStatus>
        //             {
        //                 new UserAnimeStatus
        //                 {
        //                     UserId = vm.UserId,
        //                     AnimeId = vm.AnimeId
        //                 }
        //             }
        //         };
        //
        //
        //         try
        //         {
        //             _userAnimeList.Insert(userAnimeListt);
        //             return RedirectToAction("Temp");
        //
        //         }
        //         catch (Exception e)
        //         {
        //             Console.WriteLine(e);
        //             ModelState.AddModelError(string.Empty, "An error occurred while saving the data.");
        //         }
        //     }
        //     }
        //
        //   
        // else
        // {
        //     ModelState.AddModelError(string.Empty, "User not found.");
        //
        // }

        return View(vm);
    }


    // GET
    public IActionResult Temp()
    {
        var anime = _animeRepository.List()
            .Select(anime => new Animevm
            {
                AnimeId = anime.AnimeId,
                AnimeName = anime.Name,
                ImageFileName = anime.ImageFileName,
                category = anime.AnimeCategories.Select(ac => ac.Category.Name).ToList()
            });


        return View(anime);
    }


    [HttpGet]
    public IActionResult Update(Guid Id)
    {
        var anime = _animeRepository.Find(Id);
        var cat = _categoryRepository.List();
        if (anime != null)
        {
            var categoryNames = anime.AnimeCategories
                .Select(ac => ac.Category.Name)
                .ToList();
            var vm = new Animevm
            {
                AnimeName = anime.Name,
                ImageFileName = anime.ImageFileName,
                AnimeId = anime.AnimeId,
                category = categoryNames,
                CategoryId = anime.AnimeCategories.Select(a => a.Category.CategoryId).ToList()
            };
            return View(vm);
        }

        return View();
    }

    [HttpPost]
    public IActionResult Update(Guid Id, IFormFile image, Animevm vm)
    {
        return View();
    }

    [HttpGet]
    public IActionResult Category(string categoryName)
    {
        // Your logic to fetch the list of anime for the given category
        var animeList = _categoryRepository.FindByName(categoryName);
        List<Animevm> a = new List<Animevm>();

        if (animeList != null)
        {
            var anime = animeList.AnimesCategories.Select(a => a.Anime).ToList();

            foreach (var ani in anime)
            {
                var animevm = new Animevm
                {
                    AnimeId = ani.AnimeId,
                    AnimeName = ani.Name,
                    ImageFileName = ani.ImageFileName,
                    CategoryName = animeList.Name
                };
                a.Add(animevm);
            }

            return View(a);
        }

        // Pass the anime list to the view
        return RedirectToAction("Category", new { categoryName });
    }


    public IActionResult Delete(Guid id)
    {
        var anime = _animeRepository.Find(id);
        if (anime != null)
        {
            _animeRepository.Delete(anime);
        }

        return RedirectToAction("Temp");
    }
}