using AnimeTracker.Data.Repositories.Interface;
using AnimeTracker.Models;
using AnimeTracker.ViewModel;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace AnimeTracker.Controllers;


public class UserController : Controller
{
    
    
    public INotyfService _notifyService { get; }
    private readonly IUserAnimeStatus _userAnimeStatus;

    private readonly ILogger<HomeController> _logger;
    private readonly IAnimeRepository _animeRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUsers _users;
    private readonly IUserAnimeList _userAnimeList;


    public UserController(IAnimeRepository animeRepository, ICategoryRepository categoryRepository, IUsers users,
        INotyfService notifyService, IUserAnimeStatus userAnimeStatus, IUserAnimeList userAnimeList)
    {
        _animeRepository = animeRepository;
        _categoryRepository = categoryRepository;
        _users = users;
        _notifyService = notifyService;
        _userAnimeStatus = userAnimeStatus;
        _userAnimeList = userAnimeList;
    }

    
    // GET
  [HttpGet]
    public IActionResult Login()
    {
        if (HttpContext.Session.GetString("UserName") == null)
        {
            return View();
        }
        else
        {
            var username = HttpContext.Session.GetString("UserName");
            @ViewData["user"] = username;

            var userId = _users.List()
                .Where(a => a.UserName == username)
                .Select(a => a.UserId).FirstOrDefault();
            var userRole = _users.Find(userId).Role;

            if (userRole == "user")
            {
                return RedirectToAction("Index","Home", new { area = "Anime" });

            }
            else
            {
                return RedirectToAction("UserProfile");
            }
            

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
                


                var username = HttpContext.Session.GetString("UserName");
                @ViewData["user"] = username;
                var userId = _users.List()
                    .Where(a => a.UserName == username)
                    .Select(a => a.UserId).FirstOrDefault();
                var userRole = _users.Find(userId).Role;

                if (userRole == "admin")
                {
                    return RedirectToAction("Index","Home", new { area = "Anime" });

                }
                else
                {
                    return RedirectToAction("UserProfile");
                }
            }
            // }
        }
        else
        {
            return RedirectToAction("Login");
        }

        return View();
    }
    
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
                FullName = vm.UserFullName,
                Role = "admin"
                
            };
            _users.Insert(userProfile);
            _notifyService.Success("This is a Success Login");
            return RedirectToAction("Login"); // Redirect to the login page after successful signup
        }

        _notifyService.Error("This is an Error Notification");

        return View();
    }
    
    public ActionResult Logout()
    {
        HttpContext.Session.Clear();
        HttpContext.Session.Remove("UserName");
        return RedirectToAction("Login");
    }
    
    
    
    [HttpGet]
    public IActionResult UserProfile()
    {
        var username = HttpContext.Session.GetString("UserName");
        var userId = _users.List()
            .Where(a => a.UserName == username)
            .Select(a => a.UserId).FirstOrDefault();


        if (userId == Guid.Empty)
        {
            ModelState.AddModelError(string.Empty,"No User Found");
            // Handle the case where the user was not found
            return View("Login"); // You can return an empty list or handle it differently
        }

        var userAnimeStatus = _userAnimeStatus.List()
            .Where(status => status.UserId == userId)
            .ToList();

        var animeList = new List<UserAnimeVm>();

        foreach (var status in userAnimeStatus)
        {
            // For each UserAnimeStatus, you can retrieve the associated anime
            var anime = _animeRepository.Find(status.AnimeId);
            if (anime != null)
            {
                var userAnimeList = status.UserAnimeList.UserAnimeStatus.FirstOrDefault(a => a.UserId == userId);


                var animeVm = new UserAnimeVm();
                animeVm.AnimeId = anime.AnimeId;
                animeVm.Image = anime.ImageFileName;
                animeVm.AnimeName = anime.Name;
                animeVm.Type = anime.Type;
                animeVm.ViewCount = anime.ViewCount;
                animeVm.Episode = anime.Episode;
                // Episode = userAnimeList != null ? userAnimeList.UserAnimeList.Episode : 0,
                // Add other properties you want to display
                animeList.Add(animeVm);
            }
        }


        return View(animeList);
    }

    public IActionResult Blog()
    {
        return View();
    }
}

