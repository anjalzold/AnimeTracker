using AnimeTracker.Areas.Anime.Models;
using AnimeTracker.Controllers;
using AnimeTracker.Data.Repositories.Implementation.Base;
using AnimeTracker.Data.Repositories.Interface;
using AnimeTracker.Models;
using Microsoft.EntityFrameworkCore;

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

  
 


}