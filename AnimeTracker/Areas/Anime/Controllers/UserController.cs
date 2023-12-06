using AnimeTracker.Areas.Anime.Models;
using AnimeTracker.Data.Repositories.Interface;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace AnimeTracker.Areas.Anime.Controllers;

[Area("anime")]
public class UserController : Controller
{
    public INotyfService _notifyService { get; }

    private readonly IAnimeRepository _animeRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IFileHelper _fileHelper;
    private readonly IAnimeCategory _animeCategory;
    private readonly IUsers _users;


    // GET
    public UserController(IAnimeRepository animeRepository, ICategoryRepository categoryRepository,
        IFileHelper fileHelper, IAnimeCategory animeCategory, INotyfService notifyService, IUsers users)
    {
        _animeRepository = animeRepository;
        _categoryRepository = categoryRepository;
        _fileHelper = fileHelper;
        _animeCategory = animeCategory;
        _notifyService = notifyService;
        _users = users;
    }


    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult AccountSetting()
    {
        var user = _users.List();
        var allUser = new List<Uservm>();

        foreach (var all in user)
        {
            var userAll = new Uservm();
            userAll.UserId = all.UserId;
            userAll.UserEmail = all.UserEmail;
            userAll.UserName = all.UserName;
            userAll.Role = all.Role;
            userAll.FullName = all.FullName;
            userAll.UserPassword = all.UserPassword;

            if (all.Role == "admin")
            {
                userAll.ImagePath =
                    "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRvjbwBwjCSMJPuJC4RYGoQxKh6wZAqaM6rNkQuHwHaF7joDPUAqXUldYlZ8KpFwXlwT-k&usqp=CAU";
            }
            else
            {
                userAll.ImagePath =
                    "https://e7.pngegg.com/pngimages/550/997/png-clipart-user-icon-foreigners-avatar-child-face.png";
            }

            allUser.Add(userAll);
        }

        return View(allUser);
    }


    //POST
    // [HttpPost]
    public IActionResult Delete(Guid id)
    {
        var entity = _users.Find(id);
        if (entity == null)
        {
            return NotFound();
        }


        _users.Delete(entity);
        return RedirectToAction("AccountSetting");
    }

    //GET
    [HttpGet]
    public IActionResult Update(Guid id)
    {
        var entity = _users.Find(id);
        if (entity == null)
        {
            return NotFound();
        }

        var categories = _categoryRepository.List();


        var anime = new Uservm()
        {
            UserEmail = entity.UserEmail,
            UserName = entity.UserName,
            Role = entity.Role,
            UserPassword = entity.UserPassword,
            FullName = entity.FullName,
            UserId = entity.UserId
        };

        return View(anime);
    }

    [HttpPost]

    //POST
    public IActionResult Update(Uservm vm, Guid id)
    {
        var entity = _users.Find(id);
        if (entity == null)
        {
            return NotFound();
        }


        if (vm.UserName != null)
        {
            entity.UserName = vm.UserName;
        }

        if (vm.FullName != null)
        {
            entity.FullName = vm.FullName;
        }

        if (vm.UserPassword != null)
        {
            entity.UserPassword = vm.UserPassword;
        }

        if (vm.UserEmail != null)
        {
            entity.UserEmail = vm.UserEmail;
        }

        if (vm.Role != null)
        {
            entity.Role = vm.Role;
        }


        _users.Update(entity);

        return RedirectToAction("AccountSetting");
    }
}