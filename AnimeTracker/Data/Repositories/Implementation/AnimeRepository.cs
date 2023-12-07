using AnimeTracker.Areas.Anime.Models;
using AnimeTracker.Controllers;
using AnimeTracker.Data.Repositories.Implementation.Base;
using AnimeTracker.Data.Repositories.Interface;
using AnimeTracker.Models;
using Microsoft.EntityFrameworkCore;
using F23.StringSimilarity;



namespace AnimeTracker.Data.Repositories.Implementation;

public class AnimeRepository:Repository<Anime>,IAnimeRepository
{
    public AnimeRepository(AppDbContext context) : base(context)
    {
        
        
    }
    
    public List<Anime> List()
    {
        return _context.Animes.Include(a=>a.AnimeCategories)
            .ThenInclude(a=>a.Category)
            .ToList();
    }

    
    public List<Anime> FindByName(string search,string type,string cat)
    {
        var results = _context.Animes.Where(a => a.Name.Contains(search)  || a.Type.Contains(type) || a.AnimeCategories.Select(b=>b.Category.Name).Contains(cat)).ToList();

        return results;

    }

    
    
    
    

  
 


}