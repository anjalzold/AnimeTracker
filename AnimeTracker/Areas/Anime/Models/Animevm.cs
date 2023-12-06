using AnimeTracker.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AnimeTracker.Areas.Anime.Models;

public class Animevm
{
    public string Name { get; set; }

    public IFormFile ImageFile { get; set; }

    public List<Guid> CategoryId { get; set; }
    
    public string Description { get; set; }
    
    public int ViewCount { get; set; }
    
    public Guid carsolAnimeId { get; set; }
    
    public string carsolAnimeImage { get; set; }
    
    public string carsolAnimeName { get; set; }
    
    public string carsolAnimeDescription { get; set; }


    public double AverageRating {get;set;}
    
    public string AnimeName { get; set; }
    
    public Guid AnimeId { get; set; }
    
    public Guid CatId { get; set; }

    public string CategoryName { get; set; }
    
    public int Episode { get; set; }
    
    public List<string> category { get; set; }
    
    public string Type { get; set; }
public List<SelectListItem> TypeList { get; set; }
    
    public string? ImageFileName { get; set; }
    
    public SelectList CategoryList { get; set; }

    


    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }

    
    
}