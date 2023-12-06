using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnimeTracker.Models;

public class UserAnimeStatus
{
    [Key]
    public Guid UserStatusID { get; set; }
    public Guid UserId { get; set; }
    [ForeignKey("UserId")] public virtual Users Users { get; set; }
    [ForeignKey("StatusId")] public Guid StatusId { get; set; }
    public virtual UserAnimeList UserAnimeList { get; set; }
    public Guid AnimeId { get; set; }
    [ForeignKey("AnimeId")] public virtual Anime Anime { get; set; }
}