using AnimeTracker.Areas.Anime.Models;
using AnimeTracker.Controllers;
using AnimeTracker.Data.Repositories.Interface;
using AnimeTracker.Models;
using AnimeTracker.Utils;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;

namespace AnimeTracker.Areas.Anime.Controllers;

[Area("anime")]

// [Authentication]

public class HomeController : Controller
{
    public INotyfService _notifyService { get; }

    private readonly IAnimeRepository _animeRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IFileHelper _fileHelper;
    private readonly IAnimeCategory _animeCategory;
    private readonly IUsers _users;


    // GET
    public HomeController(IAnimeRepository animeRepository, ICategoryRepository categoryRepository,
        IFileHelper fileHelper, IAnimeCategory animeCategory,INotyfService notifyService,IUsers users)
    {
        _animeRepository = animeRepository;
        _categoryRepository = categoryRepository;
        _fileHelper = fileHelper;
        _animeCategory = animeCategory;
        _notifyService = notifyService;
        _users = users;

    }



    [HttpGet]
public IActionResult Index(string search, string type, string cate, string sortOrder)
{
                ViewBag.Title = "Dashboard";

    var animelist = _animeRepository.List();
    var categoryList = _categoryRepository.List();
    var animevmList = new List<Animevm>();

    // Apply filters
    if (!string.IsNullOrEmpty(search) || !string.IsNullOrEmpty(type) || !string.IsNullOrEmpty(cate))
    {
        var results = _animeRepository.FindByName(search, type, cate);
        
        if (results.Any())
        {
            foreach (var anime in results)
            {
                var animee = new Animevm
                {
                    AnimeId = anime.AnimeId,
                    AnimeName = anime.Name,
                    ImageFileName = anime.ImageFileName,
                    Type = anime.Type,
                    ViewCount = anime.ViewCount,
                    Episode = anime.Episode,
                    category = anime.AnimeCategories.Select(ac => ac.Category.Name).ToList(),
                    Description = anime.Description
                };

                animevmList.Add(animee);
            }

            return View(animevmList);
        }
        return View(animevmList);
    }

    // Apply sorting
    if (!string.IsNullOrEmpty(sortOrder))
    {
        switch (sortOrder)
        {
            case "name_asc":
                animelist = animelist.OrderBy(a => a.Name).ToList();
                break;
            case "name_desc":
                animelist = animelist.OrderByDescending(a => a.Name).ToList();
                break;
            case "episode_asc":
                animelist = animelist.OrderBy(a => a.Episode).ToList();
                break;
            case "episode_desc":
                animelist = animelist.OrderByDescending(a => a.Episode).ToList();
                break;
        }
    }

    // Map to ViewModel
    foreach (var anime in animelist)
    {
        var categoryName = anime.AnimeCategories
            .Select(animeCat => categoryList.FirstOrDefault(catId => catId.CategoryId == animeCat.CategoryId)?.Name)
            .ToList();

        var animevm = new Animevm
        {
            AnimeId = anime.AnimeId,
            AnimeName = anime.Name,
            ImageFileName = anime.ImageFileName,
            Episode = anime.Episode,
            CategoryName = string.Join(",", categoryName),
            CurrentPage = 1,
            TotalPages = 10,
        };

        animevmList.Add(animevm);
    }

    return View(animevmList);
}



//     [HttpGet]
//     public IActionResult Index(string search,string type,string cate,string sortOrder)
//     {

//         var animelist = _animeRepository.List();
//         var categoryList = _categoryRepository.List();
//         var animevmList = new List<Animevm>();


//             if (!string.IsNullOrEmpty(search) || !string.IsNullOrEmpty(type) || !string.IsNullOrEmpty(cate))
//     {
//         // Apply the filters to the results
//         var results = _animeRepository.FindByName(search, type, cate);

//         if (results.Any())
//         {
//             // Populate the view model based on filtered results
//             foreach (var anime in results)
//             {
//                 var animee = new Animevm
//                 {
//                     AnimeId = anime.AnimeId,
//                     AnimeName = anime.Name,
//                     ImageFileName = anime.ImageFileName,
//                     Type = anime.Type,
//                     ViewCount = anime.ViewCount,
//                     Episode = anime.Episode,
//                     category = anime.AnimeCategories.Select(ac => ac.Category.Name).ToList(),
//                     Description = anime.Description
//                 };

//                 animevmList.Add(animee);
//             }

//             return View(animevmList);
//         }

//    if (!string.IsNullOrEmpty(sortOrder))
//     {
//         switch (sortOrder)
//         {
//             case "name_asc":
//                 animelist = animelist.OrderBy(a=>a.Name).ToList();
//                 break;
//             case "name_desc":
//                 animelist = animelist.OrderByDescending(a => a.Name).ToList();
//                 break;
//             case "episode_asc":
//                 animelist = animelist.OrderBy(a => a.Episode).ToList();
//                 break;
//             case "episode_desc":
//                 animelist = animelist.OrderByDescending(a => a.Episode).ToList();
           
//                 break;
//         }
//     }

//         foreach (var anime in animelist)
//         {


//             // var categoryName = anime.AnimeCategories
//             //     .Select(a => categoryList.FirstOrDefault(c => c.CategoryId == a.CategoryId)?.Name).ToList();

//             //
//             // var categoryName = anime.AnimeCategories
//             //     .Select(animeCategory =>
//             //         categoryList
//             //             .FirstOrDefault(categoryId => categoryId.CategoryId == animeCategory.CategoryId)?.Name)
//             //     .ToList();

//             var categoryName = anime.AnimeCategories
//                 .Select(animeCat => categoryList.FirstOrDefault(catId => catId.CategoryId == animeCat.CategoryId)?.Name)
//                 .ToList();
//             var animevm = new Animevm
//             {
//                 AnimeId = anime.AnimeId,
//                 AnimeName = anime.Name,
//                 ImageFileName = anime.ImageFileName,
//                 Episode = anime.Episode,
//                 CategoryName = string.Join(",", categoryName),
//                 CurrentPage = 1,
//                 TotalPages = 10,
//             };
//             animevmList.Add(animevm);
//         }


//         return View(animevmList);
//     }

    // If you are using pagination, you may want to adjust this logic accordingly



        // If no results, you may want to handle this case
    // }


        // foreach (var anime in animelist)
        // {


            // var categoryName = anime.AnimeCategories
            //     .Select(a => categoryList.FirstOrDefault(c => c.CategoryId == a.CategoryId)?.Name).ToList();

            //
            // var categoryName = anime.AnimeCategories
            //     .Select(animeCategory =>
            //         categoryList
            //             .FirstOrDefault(categoryId => categoryId.CategoryId == animeCategory.CategoryId)?.Name)
            //     .ToList();

    //         var categoryName = anime.AnimeCategories
    //             .Select(animeCat => categoryList.FirstOrDefault(catId => catId.CategoryId == animeCat.CategoryId)?.Name)
    //             .ToList();
    //         var animevm = new Animevm
    //         {
    //             AnimeId = anime.AnimeId,
    //             AnimeName = anime.Name,
    //             ImageFileName = anime.ImageFileName,
    //             Episode = anime.Episode,
    //             CategoryName = string.Join(",", categoryName),
    //             CurrentPage = 1,
    //             TotalPages = 10,
    //         };
    //         animevmList.Add(animevm);
    //     }


    //     return View(animevmList);

    // }

    // public SelectList GetCategoryOptions()
    // {
    //     
    //
    // }
    [HttpGet]
    public IActionResult Add()
    {

            ViewBag.Title = "Add";

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

        var anime = new AnimeTracker.Models.Anime();
        anime.Name = vm.Name;
        anime.ImageFileName = vm.ImageFileName;
        anime.Episode = vm.Episode;
        anime.Type = vm.Type;
        anime.Description = vm.Description;
        anime.AnimeCategories = vm.CategoryId.Select(category => new AnimeCategory
        {
            CategoryId = category
        }).ToList();

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
                    ViewBag.Title = "Update Anime";

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
            Episode = entity.Episode,
            AnimeName = entity.Name,
            Image = entity.ImageFileName,
            Type = entity.Type,
            CategoryId = animeCategories,
            Description = entity.Description
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
        if (vm.Description != null)
        {
            entity.Description = vm.Description;
        }

        if (vm.Episode != null)
        {
            entity.Episode = vm.Episode;
        }

        if (vm.Image != null)
        {
            entity.ImageFileName = vm.Image;
        }
        if (vm.Type != null)
        {
            entity.Type = vm.Type;
        }


        if (vm.CategoryId != null)
        {
            var existingCategories = entity.AnimeCategories.Select(ac => ac.CategoryId).ToList();

            var categoriesToRemove = entity.AnimeCategories.Where(ac => !vm.CategoryId.Contains(ac.CategoryId)).ToList();
            foreach (var categoryToRemove in categoriesToRemove)
            {
                entity.AnimeCategories.Remove(categoryToRemove);
            }
            

            foreach (var categoryId in vm.CategoryId)
            {
                if (!existingCategories.Contains(categoryId))
                {
                    entity.AnimeCategories.Add(new AnimeCategory
                    {
                        AnimeId = vm.AnimeId,
                        CategoryId = categoryId
                    });
                }
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


 
    
    
    public ActionResult Logout()
    {
        HttpContext.Session.Clear();
        HttpContext.Session.Remove("UserName");
        return RedirectToAction("Login","User",new{area=""});
    }
}