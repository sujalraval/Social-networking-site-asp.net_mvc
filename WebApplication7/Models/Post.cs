using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication7.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string ImagePath { get; set; }
        public DateTime PostedOn { get; set; }
        // total likes
        public virtual ICollection<Like> Likes { get; set; }
        public int TotalLikes { get; set; }

        // Foreign key to User
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public string VideoPath { get; set; }
    }

    public class Like
    {
        public int Id { get; set; }

        // Foreign key to Post
        public int PostId { get; set; }
        public Post Post { get; set; }

        // Foreign key to User
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }

    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }

        // Foreign key to Post
        public int PostId { get; set; }
        public Post Post { get; set; }

        // Foreign key to User
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime CommentedOn { get; set; } = DateTime.Now;// Timestamp for when the comment was made

    }

}