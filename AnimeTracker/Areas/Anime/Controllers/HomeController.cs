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
        var animelist = _animeRepository.List();
        var categoryList = _categoryRepository.List();
        var animevmList = new List<Animevm>();

        foreach (var anime in animelist)
        {
            // var categoryName = anime.AnimeCategories
            //     .Select(a => categoryList.FirstOrDefault(c => c.CategoryId == a.CategoryId)?.Name).ToList();

            //
            // var categoryName = anime.AnimeCategories
            //     .Select(animeCategory =>
            //         categoryList
            //             .FirstOrDefault(categoryId => categoryId.CategoryId == animeCategory.CategoryId)?.Name)
            //     .ToList();

            var categoryName = anime.AnimeCategories
                .Select(animeCat => categoryList.FirstOrDefault(catId => catId.CategoryId == animeCat.CategoryId)?.Name)
                .ToList();
            var animevm = new Animevm
            {
                AnimeId = anime.AnimeId,
                AnimeName = anime.Name,
                ImageFileName = anime.ImageFileName,
                CategoryName = string.Join(",", categoryName)
            };
            animevmList.Add(animevm);
        }


        return View(animevmList);
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
    public IActionResult Add(Animevm vm, IFormFile ImageFile)
    {
        if (ImageFile != null && ImageFile.Length > 0)
        {
            string imagePath = "images";
            string uniqueFileName = _fileHelper.SaveFileAndReturnName(imagePath, ImageFile);
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

        _fileHelper.Delete("images", entity.ImageFileName);

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

        var categories = _categoryRepository.List();

        var animeCategories = entity.AnimeCategories != null
            ? entity.AnimeCategories.Select(ac => ac.CategoryId).ToList()
            : new List<Guid>();


        var anime = new UpdateAnimevm
        {
            CategoryList = new SelectList(categories, "CategoryId", "Name"),
            AnimeName = entity.Name,
            Image = entity.ImageFileName,
            CategoryId = animeCategories
        };

        return View(anime);
    }

    [HttpPost]

    //POST
    public IActionResult Update(UpdateAnimevm vm, Guid id, IFormFile ImageFile)
    {
        var entity = _animeRepository.Find(id);
        if (entity == null)
        {
            return NotFound();
        }

        if (ImageFile != null && ImageFile.Length > 0)
        {
            string imagePath = "images";
            _fileHelper.Delete(imagePath, entity.ImageFileName);
            string uniqueFileName = _fileHelper.SaveFileAndReturnName(imagePath, ImageFile);
            vm.Image = uniqueFileName;
        }


        if (vm.AnimeName != null)
        {
            entity.Name = vm.AnimeName;
        }

        if (vm.Image != null)
        {
            entity.ImageFileName = vm.Image;
        }


        if (vm.CategoryId != null)
        {
            entity.AnimeCategories = new List<AnimeCategory>();

            foreach (var categoryId in vm.CategoryId)
            {
                entity.AnimeCategories.Add(new AnimeCategory
                {
                    AnimeId = vm.AnimeId,
                    CategoryId = categoryId
                });
            }
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

    [HttpGet]
    public IActionResult GetCategory()
    {
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

        return RedirectToAction("Index");
    }
}