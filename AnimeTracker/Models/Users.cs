using System.ComponentModel.DataAnnotations;

namespace AnimeTracker.Models;

public class Users
{
    [Key]
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public string UserEmail { get; set; }
    public string UserPassword { get; set; }
    
    public string FullName { get; set; }
    
    public string Role { get; set; }
    
    
    
    public virtual List<UserAnimeStatus> UserAnimeStatus { get; set; }
}