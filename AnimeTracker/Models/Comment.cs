using System.ComponentModel.DataAnnotations;
using AnimeTracker.Models;

public class Comment
{
    [Key]
    public Guid CommentId { get; set; }
    public Guid UserId { get; set; }
    public virtual Users User { get; set; }

            public string UserName { get; set; } // Include the username directly in the Comment table

    public string CommentText { get; set; }
    public DateTime CommentDate { get; set; }
        public Guid AnimeId { get; set; }  // Add AnimeId to associate comments with an anime


        public virtual Anime Anime { get; set; }

}
