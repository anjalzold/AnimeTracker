using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnimeTracker.Models
{
    [Table("Animes")]
    public class Anime
    {
        [Key]
        public Guid AnimeId { get; set; }

        [Required] public string Name { get; set; }

        public string? ImageFileName { get; set; }
        
        public int Episode { get; set; }

        public string? comment {get;set;}
        
        public string Description { get; set; }
        
        public string? Type { get; set; }
        
        public int ViewCount { get; set; }
        public double AverageRating { get; set; }
        public int TotalRatings { get; set; }

        public virtual List<Comment> Comments { get; set; }


        
        public virtual List<AnimeCategory> AnimeCategories { get; set; }
    }
}