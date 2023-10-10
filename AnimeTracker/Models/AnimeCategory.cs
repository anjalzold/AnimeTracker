using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnimeTracker.Models
{
    [Table("AnimeCategories")]
    public class AnimeCategory
    {
        [Key] public Guid AnimeCategoryId { get; set; }

        public Guid AnimeId { get; set; }

        [ForeignKey("AnimeId")]
        public virtual Anime Anime { get; set; }

        public Guid CategoryId { get; set; }

        [ForeignKey("CategoryId")] 
        public virtual Category Category { get; set; }
    }
}