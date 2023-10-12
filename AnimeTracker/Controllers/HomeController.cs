using System.Diagnostics;
using AnimeTracker.Areas.Anime.Models;
using AnimeTracker.Data.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using AnimeTracker.Models;
using AnimeTracker.ViewModel;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace AnimeTracker.Controllers;

public class HomeController : Controller
{
    public INotyfService _notifyService { get; }

    private readonly ILogger<HomeController> _logger;
    private readonly IAnimeRepository _animeRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUsers _users;


    public HomeController(IAnimeRepository animeRepository, ICategoryRepository categoryRepository, IUsers users,
        INotyfService notifyService)
    {
        _animeRepository = animeRepository;
        _categoryRepository = categoryRepository;
        _users = users;
        _notifyService = notifyService;
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

    // [HttpGet]
    // public IActionResult Login()
    // {
    //
    //
    //     return View();
    // }


    [HttpGet]
    public IActionResult Login()
    {
        if (HttpContext.Session.GetString("UserName") == null)
        {
            return View();
        }
        else
        {
            return RedirectToAction("Index");
        }
    }


    [HttpPost]
    public ActionResult Login(UserVm vm)
    {
        if (HttpContext.Session.GetString("UserName") == null)
        {
            // if (ModelState.IsValid)
            // {
                var obj = _users.List()
                    .FirstOrDefault(a => a.UserName == vm.UserName && a.UserPassword == vm.UserPassword);

                if (obj != null)
                {
                    HttpContext.Session.SetString("UserName", obj.UserName);
                    return RedirectToAction("Index",new{area="Anime"});
                }
            // }
        }
        else
        {
            return RedirectToAction("Login");
        }

        return View();
    }
    //
    // [HttpPost]
    // public IActionResult Login(UserVm vm)
    // {
    //     // if (ModelState.IsValid)
    //     // {
    //         var login = _users.List()
    //             .FirstOrDefault(a => a.UserName == vm.UserName && a.UserPassword == vm.UserPassword);
    //
    //         if (login != null)
    //         {
    //             _notifyService.Success("Login Success");
    //
    //             
    //             return RedirectToAction("Index",new{area="Anime"});
    //
    //
    //             
    //
    //
    //         }
    //
    //         _notifyService.Error("Login Failed");
    //     // }
    //
    //
    //     return View(vm);
    // }

    [HttpGet]
    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost]
    public IActionResult SignUp(UserVm vm)
    {
        if (ModelState.IsValid)
        {
            var userWithUsername = _users.List().FirstOrDefault(u => u.UserEmail == vm.UserEmail);
            var userWithEmail = _users.List().FirstOrDefault(u => u.UserName == vm.UserName);

            if (userWithEmail != null)
            {
                ModelState.AddModelError("UserEmail", "Email is already registered.");
                return View();
            }

            if (userWithUsername != null)
            {
                // Username is already in use
                ModelState.AddModelError("UserName", "Username is already in use.");
                return View();
            }

            if (userWithEmail != null || userWithUsername != null)
            {
                // At least one of the fields (email or username) is already in use
                ModelState.AddModelError("UserEmail", "Email is already registered.");
                ModelState.AddModelError("UserName", "Username is already in use.");
                return View();
            }

            if (vm.UserPassword != vm.RepeatPassword)
            {
                ModelState.AddModelError("UserName", "Username is already in use.");
                _notifyService.Error("Please Enter same password");

                return View();
            }

            var userProfile = new Users
            {
                UserEmail = vm.UserEmail,
                UserPassword = vm.UserPassword,
                UserName = vm.UserName,
                FullName = vm.UserFullName
            };
            _users.Insert(userProfile);
            _notifyService.Success("This is a Success Login");
            return RedirectToAction("Login"); // Redirect to the login page after successful signup
        }

        _notifyService.Error("This is an Error Notification");

        return View();
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