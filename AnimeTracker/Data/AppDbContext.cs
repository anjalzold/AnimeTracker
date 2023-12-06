using AnimeTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AnimeTracker.Data;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions options):base(options)
    {
        
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //To Display the Generated the Database Script
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);

        //Enabling Lazy Loading
        optionsBuilder.UseLazyLoadingProxies();

    }
    
    
    public DbSet<Anime> Animes { get; set; }
    
    public DbSet<Category> Categories { get; set; }
    
    public DbSet<AnimeCategory> AnimeCategories { get; set; }
    
    public DbSet<Users> Users { get; set; }
    
    public DbSet<UserAnimeList> UserAnimeLists { get; set; }
    public DbSet<UserAnimeStatus> UserAnimeStatuss { get; set; }

    



}