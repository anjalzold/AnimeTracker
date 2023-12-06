using AnimeTracker.Enums;

namespace AnimeTracker.ViewModel;

public class UserAnimeVm
{
    public string AnimeName { get; set; }
    
    public Guid AnimeId { get; set; }
    
    public int ViewCount { get; set; }
    
    public double Ratings { get; set; }
    
    public int TotalRatings { get; set; }

    public string Comment { get; set; }

        public List<(string UserName, string Comment)> CommentDetails { get; set; }



    public List<string> Comments { get; set; }

    

    
    public string Type { get; set; }
    public string Description { get; set; }

    
    public Guid UserId { get; set; }
    public int Episode { get; set; }
    public Status Status { get; set; }
    public string UserName { get; set; }

    public List<string> users { get; set; }
    
    public List<string> category { get; set; }
    public List<Guid> CategoryId { get; set; }
    
    public int TotalEpisode { get; set; }

    
    public string Image { get; set; }


}