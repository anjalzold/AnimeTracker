using AnimeTracker.Areas.Anime.Models;
using AnimeTracker.Controllers;
using AnimeTracker.Data.Repositories.Interface;
using AnimeTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AnimeTracker.Areas.Anime.Controllers;

[Area("anime")]
public class HomeController : Controller
{
    private readonly IAnimeRepository _animeRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IFileHelper _fileHelper;
    private readonly IAnimeCategory _animeCategory;


    // GET
    public HomeController(IAnimeRepository animeRepository, ICategoryRepository categoryRepository,
        IFileHelper fileHelper, IAnimeCategory animeCategory)
    {
        _animeRepository = animeRepository;
        _categoryRepository = categoryRepository;
        _fileHelper = fileHelper;
        _animeCategory = animeCategory;
    }


    [HttpGet]
    public IActionResult Index()
    {
        List<AnimeTracker.Models.Anime> a = _animeRepository.List();


        return View(a);
    }

    // public SelectList GetCategoryOptions()
    // {
    //     
    //
    // }
    [HttpGet]
    public IActionResult Add()
    {
        var categories = _categoryRepository.List();
        var categoryOptions = new SelectList(categories, nameof(Category.CategoryId), nameof(Category.Name));
        var vm = new Animevm
        {
            CategoryList = categoryOptions
        };


        return View(vm);


        //
        // public SelectList GetCategoryOptions()
        //     => new SelectList(Categories, nameof(Category.Id), nameof(Category.Name), CategoryId);
        // Kushal Niroula20:29
        //     <select asp-items="@Model.GetCategoryOptions()">.
    }


    //POST
    public IActionResult Add(Animevm vm, IFormFile imageFile)
    {
        if (imageFile != null && imageFile.Length > 0)
        {
            string imagePath = "images";
            string uniqueFileName = _fileHelper.SaveFileAndReturnName(imagePath, imageFile);
            vm.ImageFileName = uniqueFileName;
        }

        var anime = new AnimeTracker.Models.Anime
        {
            Name = vm.Name,
            ImageFileName = vm.ImageFileName,
            AnimeCategories = vm.CategoryId.Select(category => new AnimeCategory
            {
                CategoryId = category
            }).ToList()
        };

        _animeRepository.Insert(anime);


        return RedirectToAction("Index");
    }

    //POST
    // [HttpPost]
    public IActionResult Delete(Guid id)
    {
        var entity = _animeRepository.Find(id);
        if (entity == null)
        {
            return NotFound();
        }

        _animeRepository.Delete(entity);
        return RedirectToAction("Index");
    }

    //GET
    [HttpGet]
    public IActionResult Update(Guid id)
    {
        AnimeTracker.Models.Anime entity = _animeRepository.Find(id);
        if (entity == null)
        {
            return NotFound();
        }

        List<Category> categories = _categoryRepository.List();

        var animeCategories = entity.AnimeCategories != null
            ? entity.AnimeCategories.Select(ac => ac.CategoryId).ToList()
            : new List<Guid>(); 


        var anime = new UpdateAnimevm
        {
            CategoryList = new SelectList(categories, "CategoryId", "Name"),
            Name = entity.Name,
            CategoryId = animeCategories
            
        };

        return View(anime);
    }

    [HttpPost]

    //POST
    public IActionResult Update(UpdateAnimevm vm, Guid id, IFormFile imageFile)
    {

        var entity = _animeRepository.Find(id);
        if (entity ==null)
        {
            return NotFound();

        }
        
        if (imageFile != null && imageFile.Length > 0)
        {
            string imagePath = "images";
            string uniqueFileName = _fileHelper.SaveFileAndReturnName(imagePath, imageFile);
            vm.Image = uniqueFileName;
        }

        entity.Name = vm.Name;

        if (imageFile == null)
        {
            vm.Image = entity.ImageFileName;
        }
        entity.ImageFileName = vm.Image;
        entity.AnimeCategories = new List<AnimeCategory>();

        
        foreach (var categoryId in vm.CategoryId)
        {
            entity.AnimeCategories.Add(new AnimeCategory
            {
                AnimeId = vm.AnimeId,
                CategoryId = categoryId
            });
            
        }

        _animeRepository.Update(entity);
        
        // var entity = _animeRepository.Find(id);
        // if (entity == null)
        // {
        //     return NotFound();
        // }
        //
        // entity.Name = vm.Name;
        // entity.AnimeCategories = new List<AnimeCategory>();
        // foreach (var categoryId in vm.CategoriesId)
        // {
        //     entity.AnimeCategories.Add(new AnimeCategory
        //     {
        //         AnimeId = entity.AnimeId,
        //         CategoryId = categoryId
        //     });
        //     
        //     
        // }
   


        // if (imageFile != null && imageFile.Length > 0)
        // {
        //     string imagePath = "images";
        //     string uniqueFileName = _fileHelper.SaveFileAndReturnName(imagePath, imageFile);
        //     entity.Anime.ImageFileName = uniqueFileName;
        // }
        //
        // if (vm.AnimeCategory != null)
        // {
        //     if (vm.AnimeCategory.Anime != null)
        //     {
        //         entity.Anime.Name = vm.AnimeCategory.Anime.Name;
        //     }
        //
        //     if (vm.AnimeCategory.Category != null)
        //     {
        //         entity.Category.Name = vm.AnimeCategory.Category.Name;
        //     }
        // }
        //
        //
        // _animeCategory.Update(entity);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult AddCategory()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddCategory(Animevm cm)
    {
        var vm = new Category
        {
            Name = cm.CategoryName
        };
        _categoryRepository.Insert(vm);

        return RedirectToAction("Index");
    }
}