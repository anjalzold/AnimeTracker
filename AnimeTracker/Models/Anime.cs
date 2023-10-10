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

        public string ImageFileName { get; set; }

        
        public virtual List<AnimeCategory> AnimeCategories { get; set; }
    }
}