using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication7.Models
{
    public class Activity
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public ActivityType Type { get; set; }

        // Additional properties based on activity type
        public int? PostId { get; set; }
        public int? CommentId { get; set; }
        public int? LikedUserId { get; set; }
        public int? UnlikedUserId { get; set; }
        public int? UnfollowedUserId { get; set; }
        public string ViewedUserId { get; set; } // Change the type to string
        // Add more properties as needed

        // Navigation properties
        public virtual ApplicationUser User { get; set; }
        public virtual Post Post { get; set; }
        public virtual Comment Comment { get; set; }
        public virtual ApplicationUser LikedUser { get; set; }
        public virtual ApplicationUser UnlikedUser { get; set; }
        public virtual ApplicationUser UnfollowedUser { get; set; }
        public virtual ApplicationUser ViewedUser { get; set; }
    }

    public enum ActivityType
    {
        Comment,
        Like,
        Unlike,
        Follow,
        Unfollow,
        //ViewProfile,
        EditProfile,
        ViewProfile,
        // Add more activity types as needed
    }

}