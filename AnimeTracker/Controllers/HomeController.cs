using System.Diagnostics;
using AnimeTracker.Areas.Anime.Models;
using AnimeTracker.Data.Repositories.Interface;
using AnimeTracker.Enums;
using Microsoft.AspNetCore.Mvc;
using AnimeTracker.Models;
using AnimeTracker.ViewModel;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;

namespace AnimeTracker.Controllers;

public class HomeController : Controller
{
    public INotyfService _notifyService { get; }
    private readonly IUserAnimeStatus _userAnimeStatus;

    private readonly ILogger<HomeController> _logger;
    private readonly IAnimeRepository _animeRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUsers _users;
    private readonly IUserAnimeList _userAnimeList;


    public HomeController(IAnimeRepository animeRepository, ICategoryRepository categoryRepository, IUsers users,
        INotyfService notifyService, IUserAnimeStatus userAnimeStatus, IUserAnimeList userAnimeList)
    {
        _animeRepository = animeRepository;
        _categoryRepository = categoryRepository;
        _users = users;
        _notifyService = notifyService;
        _userAnimeStatus = userAnimeStatus;
        _userAnimeList = userAnimeList;
    }


    public IActionResult NoConnection()
    {
        return View();
    }

    public IActionResult Index(string search,string type,string cate)
    {

        try
        {
            _animeRepository.List();

        }
        catch (SqlException ex)
        {
            return RedirectToAction("NoConnection");
        }

        
        
        

      
        
        
        var animeList = new List<Animevm>();

       


        if (!string.IsNullOrEmpty(search) || !string.IsNullOrEmpty(type) || !string.IsNullOrEmpty(cate))
        {
            var results = _animeRepository.FindByName(search,type,cate);
            if (results != null)
            {
                foreach (var anime in results)
                {
                    var animee = new Animevm();
                    animee.AnimeId = anime.AnimeId;
                    animee.AnimeName = anime.Name;
                    animee.ImageFileName = anime.ImageFileName;
                    animee.Type = anime.Type;
                    animee.ViewCount = anime.ViewCount;
                    animee.Episode = anime.Episode;
                    animee.category = anime.AnimeCategories.Select(ac => ac.Category.Name).ToList();
                    animee.Description = anime.Description;
                    animee.AverageRating = anime.AverageRating;


                    animeList.Add(animee);
                }

                return View(animeList);
            }

            return RedirectToAction("Index");
        }
        
        
        
        
        

        var animes = _animeRepository.List();
        var category = _categoryRepository.List();


        foreach (var anime in animes)
        {
            var animee = new Animevm();
            animee.AnimeId = anime.AnimeId;
            animee.AnimeName = anime.Name;
            animee.ImageFileName = anime.ImageFileName;
            animee.Type = anime.Type;
            animee.category = anime.AnimeCategories.Select(ac => ac.Category.Name).ToList();
            animee.ViewCount = anime.ViewCount;
            animee.Episode = anime.Episode;
            animee.Description = anime.Description;
            animee.AverageRating = anime.AverageRating;

            animeList.Add(animee);
        }

        return View(animeList);
    }

    // [HttpGet]
    // public IActionResult Login()
    // {
    //
    //
    //     return View();
    // }


   
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

   

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet]
    public IActionResult AnimeDetail(Guid id)
    {
        
        var animeName = _animeRepository.Find(id);
        if(animeName!=null){
 animeName.ViewCount++;
        _animeRepository.Update(animeName);
        }
       
        
        var username = HttpContext.Session.GetString("UserName");
        var userId = _users.List()
            .Where(a => a.UserName == username)
            .Select(a => a.UserId).FirstOrDefault();
        
      


        var animes = _animeRepository.Find(id);
        var cat = _categoryRepository.List();

        var average = _userAnimeStatus.List().Where(a => a.AnimeId == id).Select(a => a.UserAnimeList).Select(a=>a.TotalRatings).Average();

        var animeRate = _animeRepository.List().Where(a => a.AnimeId == id).Select(a => a.AverageRating).FirstOrDefault();

        if (average > 0)
        {
            animes.AverageRating = average;
            _animeRepository.Update(animes);
        }
        else
        {
            animes.AverageRating = animeRate;
            _animeRepository.Update(animes);
        }







        var categoryNames = animes.AnimeCategories
            .Select(ac => ac.Category.Name)
            .ToList();
        
        var animename = animes.Name;
        var totalView = animes.ViewCount;
        var images = animes.ImageFileName;
        var totalEpisode = animes.Episode;
        


        Status status; // Default status

        if (animes == null)
        {
            return View();
        }

var existingUserAnimeList = _userAnimeList.List()
                .FirstOrDefault(u => u.UserAnimeStatus.Any(ui => ui.UserId == userId && ui.AnimeId == animes.AnimeId));

                List<string> users = new List<string>();

   
List<string> comments = new List<string>();
List<string> commentUser = new List<string>();

var userAnimeStatusInfo = _userAnimeStatus.List()
    .Where(ui => ui.AnimeId == animes.AnimeId)  // Remove the condition for the current user
    .Select(ui => new { ui.UserId, ui.UserAnimeList.Comments })
    .ToList();


List<(string UserName, string Comment)> commentDetails = new List<(string, string)>();

if (animes != null)
{
      var commentId = animes.Comments
        .Select(comment => (comment.User.UserName, comment.CommentText))
        .ToList();

    commentDetails.AddRange(commentId);
}







        if (userId != Guid.Empty)
        {
            var userAnimeStatus = _userAnimeStatus.List()
                .FirstOrDefault(status => status.UserId == userId && status.AnimeId == id);
            
            
                

            if (userAnimeStatus != null)
            {
                status = userAnimeStatus.UserAnimeList.Status;
                var list = new UserAnimeVm();
                list.UserId = userId;
                list.UserName = username;
                list.AnimeId = id;
                list.AnimeName = animename;
                list.Image = images;
                list.Episode = userAnimeStatus.UserAnimeList.Episode;
                list.Status = status;
                list.category = categoryNames;
                list.CategoryId = animes.AnimeCategories.Select(a => a.Category.CategoryId).ToList();
                list.TotalEpisode = totalEpisode;
                list.ViewCount = totalView;
                list.TotalRatings = existingUserAnimeList.TotalRatings;
                list.Ratings = Math.Round(average,2);
                list.Comments = comments;
                list.Comment = "";
                list.users = commentUser;
                list.CommentDetails = commentDetails;
                return View(list);
            }
        }

        List<string> BlankComment = new List<string>();

        var anime = new UserAnimeVm
        {
            UserId = userId,
            AnimeId = id,
            AnimeName = animename,
            Image = images,
            Episode = 0,
            category = categoryNames,
            CategoryId = animes.AnimeCategories.Select(a => a.Category.CategoryId).ToList(),
            TotalEpisode = totalEpisode,
            Description = animes.Description,
            ViewCount = totalView,
            TotalRatings = 0,
            Ratings = Math.Round(average,2),
            Comments = BlankComment,
            Comment = "",
            users = commentUser,
            UserName = username,
            CommentDetails = commentDetails


        };

        return View(anime);
    }

    [HttpPost]
    public IActionResult AnimeDetail(UserAnimeVm vm, Guid id)
    {


int average = vm.TotalRatings;

        if (vm.UserId == Guid.Empty)
        {
            ModelState.AddModelError(string.Empty, "You are not signed in and need to Sign in to save Progress.");
            return View(vm);
        }

        if(vm.TotalRatings > 10)
        {
            ModelState.AddModelError(string.Empty, "Ratings cannot be more than 10");
            return View(vm);
        }

        // Check if the user is updating their status
        if (vm.UserId != Guid.Empty)
        {
      

            var existingUserAnimeList = _userAnimeList.List()
                .FirstOrDefault(u => u.UserAnimeStatus.Any(ui => ui.UserId == vm.UserId && ui.AnimeId == vm.AnimeId));

            if (existingUserAnimeList != null)
            {
                // If the anime is already in the user's list, update the status and episode
                existingUserAnimeList.Status = vm.Status;
                existingUserAnimeList.Episode = vm.Episode;
                existingUserAnimeList.TotalRatings = vm.TotalRatings;
                
                

                try
                {
                    _userAnimeList.Update(existingUserAnimeList);
                    return RedirectToAction("Index");
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
                    TotalRatings = average,
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
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while saving the data.");
                    Console.WriteLine(e);
                    return View(vm);
                }
            }
        }

        return View(vm);
    }
    
    
 

    [HttpGet]
    public IActionResult Category(string categoryName)
    {
        // Your logic to fetch the list of anime for the given category
        var animeList = _categoryRepository.FindByName(categoryName);
        List<Animevm> a = new List<Animevm>();
        
        var categoryNames = _categoryRepository.List();

        var cat = categoryNames.Select(cat => cat.Name).ToList();

        if (animeList != null)
        {
            var anime = animeList.AnimesCategories.Select(a => a.Anime).ToList();

            foreach (var ani in anime)
            {
                var animevm = new Animevm();
                animevm.AnimeId = ani.AnimeId;
                animevm.AnimeName = ani.Name;
                animevm.ImageFileName = ani.ImageFileName;
                animevm.CategoryName = animeList.Name;
                animevm.Type = ani.Type;
                animevm.Episode = ani.Episode;
                animevm.ViewCount = ani.ViewCount;
                animevm.category = cat;
                

                a.Add(animevm);
            }

            return View(a);
        }

        // Pass the anime list to the view
        return RedirectToAction("Category", new { categoryName });
    }

 [HttpPost]
public ActionResult AddComment(UserAnimeVm vm)
{
    if (vm.UserId != Guid.Empty)
    {
        var newComment = new Comment
        {
            UserId = vm.UserId,
            CommentText = vm.Comment,
            CommentDate = DateTime.Now,
            UserName = vm.UserName,
        };

        try
        {
            var userAnimeList = _animeRepository.Find(vm.AnimeId);
            if (userAnimeList != null)
            {
                userAnimeList.Comments ??= new List<Comment>();
                userAnimeList.Comments.Add(newComment);

                // Update the UserAnimeList entity with the new comment
                _animeRepository.Update(userAnimeList);

                // Redirect back to AnimeDetail with the correct id parameter
                return RedirectToAction("AnimeDetail", new { id = vm.AnimeId });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "UserAnimeList not found.");
            }
        }
        catch (Exception e)
        {
            ModelState.AddModelError(string.Empty, $"An error occurred while saving the data: {e.Message}");
            Console.WriteLine(e);
        }
    }
    else
    {
        ModelState.AddModelError(string.Empty, "Invalid UserId.");
    }

    // If there's an error, return to the AnimeDetail page without updating
    return View("AnimeDetail", vm);
}


    
    [HttpGet]

    public IActionResult CategoryPage()
    {
        List<Animevm> a = new List<Animevm>();

        var categoryNames = _categoryRepository.List();

        var cat = categoryNames.Select(cat => cat.Name).ToList();

        var anime = new Animevm();
        anime.category = cat;

        return View(anime);

    }
    
    public IActionResult NotFound(int? statusCode)
    {
        if (statusCode.HasValue && statusCode.Value == 404)
        {
            return View("NotFound");
        }

        return View("Error");
        
    }

  



    //
    // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    // public IActionResult Error()
    // {
    //     return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    // }
}