using AnimeTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AnimeTracker.Data;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions options):base(options)
    {
        
    }
    
    public DbSet<Anime> Animes { get; set; }
    
    public DbSet<Category> Categories { get; set; }
    
    public DbSet<AnimeCategory> AnimeCategories { get; set; }

    



}