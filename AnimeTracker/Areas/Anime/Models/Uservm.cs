namespace AnimeTracker.Areas.Anime.Models;

public class Uservm
{
    public Guid UserId { get; set; }
 
    public string UserName { get; set; }
    public string UserEmail { get; set; }
    public string UserPassword { get; set; }
    
    public string FullName { get; set; }
    
    public string Role { get; set; }
    
    public string ImagePath { get;set; }
    

}