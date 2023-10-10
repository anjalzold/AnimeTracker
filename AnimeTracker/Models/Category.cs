using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnimeTracker.Models
{
    [Table("Categories")]
    public class Category
    {
        public Guid CategoryId { get; set; }
    
        [Required]
        public string Name { get; set; }
        
        public virtual List<AnimeCategory> AnimesCategories { get; set; }
    
        // Navigation property for the many-to-many relationship with Anime
    }
}

