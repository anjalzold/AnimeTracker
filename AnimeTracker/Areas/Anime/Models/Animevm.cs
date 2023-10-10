using AnimeTracker.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AnimeTracker.Areas.Anime.Models;

public class Animevm
{
    public string Name { get; set; }

    public IFormFile ImageFile { get; set; }

    public List<Guid> CategoryId { get; set; }
    
    public string AnimeName { get; set; }
    
    public Guid CatId { get; set; }

    public string CategoryName { get; set; }
    

    
    public string ImageFileName { get; set; }
    
    public SelectList CategoryList { get; set; }

    
    
}