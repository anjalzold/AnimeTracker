using System.ComponentModel.DataAnnotations;
using AnimeTracker.Enums;

namespace AnimeTracker.Models;

public class UserAnimeList
{
    [Key]
    public Guid Id { get; set; }
    public int Episode { get; set; }
    public int TotalRatings { get; set; }

    public virtual List<Comment> Comments { get; set; }
    public Status Status { get; set; }
    public virtual List<UserAnimeStatus> UserAnimeStatus { get; set; }
}