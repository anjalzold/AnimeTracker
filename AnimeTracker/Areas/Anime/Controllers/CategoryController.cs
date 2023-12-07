using AnimeTracker.Areas.Anime.Models;
using AnimeTracker.Data.Repositories.Interface;
using AnimeTracker.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace AnimeTracker.Areas.Anime.Controllers;

[Area("anime")]

public class CategoryController : Controller
{
    
    public INotyfService _notifyService { get; }

    private readonly IAnimeRepository _animeRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IFileHelper _fileHelper;
    private readonly IAnimeCategory _animeCategory;
    private readonly IUsers _users;


    // GET
    public CategoryController(IAnimeRepository animeRepository, ICategoryRepository categoryRepository,
        IFileHelper fileHelper, IAnimeCategory animeCategory,INotyfService notifyService,IUsers users)
    {
        _animeRepository = animeRepository;
        _categoryRepository = categoryRepository;
        _fileHelper = fileHelper;
        _animeCategory = animeCategory;
        _notifyService = notifyService;
        _users = users;

    }
    // GET
  
    [HttpGet]
    public IActionResult AddCategory()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddCategory(Animevm cm)
    {
                    ViewBag.Title = "Add Category";

        var vm = new Category
        {
            Name = cm.CategoryName
        };
        _categoryRepository.Insert(vm);

        return RedirectToAction("Add", "Home");
    }

    [HttpGet]
    public IActionResult GetCategory()
    {
                    ViewBag.Title = "View Category";

        var categories = _categoryRepository.List();

        // Create an Animevm instance to hold the categories

        var category = new List<Animevm>();
        foreach (var categorys in categories)
        {
            var categoryName = categorys.Name;
            var categoryId = categorys.CategoryId;
            var animeCategory = new Animevm
            {
                CategoryName = categoryName,
                CatId = categoryId
            };
            category.Add(animeCategory);
        }

        return View(category);
    }


    public IActionResult DeleteCategory(Guid id)
    {
        var catgeoryID = _categoryRepository.Find(id);
        if (catgeoryID != null)
        {
            _categoryRepository.Delete(catgeoryID);
        }

        return RedirectToAction("Index","Home");
    }
}