using Microsoft.AspNetCore.Mvc.Rendering;

namespace AnimeTracker.Areas.Anime.Models;

public class UpdateAnimevm
{
    public int Id { get; set; }

    public Guid AnimeId { get; set; }

    public string AnimeName { get; set; }
    public string? Image { get; set; }
    
    public string Description { get; set; }

    
    public int Episode { get; set; }
    
    public string Type { get; set; }

    public List<Guid> CategoryId { get; set; }

    public SelectList CategoryList { get; set; }
}