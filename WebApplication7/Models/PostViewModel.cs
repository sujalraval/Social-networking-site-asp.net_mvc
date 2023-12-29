using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Xunit;

namespace WebApplication7.Models
{
    public class PostViewModel
    {
        public List<string> Notifications { get; set; }
        public List<Post> Posts { get; set; }
        public string CurrentUserId { get; set; }
        public string NewPostText { get; set; }
        public List<int> LikedPostIds { get; set; } // Store the post IDs that the current user has liked
        public List<SearchResultViewModel> SearchResults { get; set; } // Corrected property type
                                                                       //public virtual ICollection<Comment> Comments { get; set; }
        public List<CommentViewModel> Comments { get; set; }

        public int Id { get; set; } // Add this property for PostId
        public List<ApplicationUser> Likes { get; set; }
        public List<ApplicationUser> Followings { get; internal set; }
        public int FollowingCount { get; set; }
        public List<ApplicationUser> Followers { get; internal set; }
        public int FollowersCount { get; set; }
        //public List<SearchResultViewModel> SearchResults { get; set; }

    }


    public class CommentViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ProfileImage { get; set; }
        public string Text { get; set; }
        public DateTime CommentedOn { get; set; }

        // Other properties...
    }

    public class EditPostViewModel
    {
        public int PostId { get; set; }

        [Required(ErrorMessage = "Please enter text.")]
        public string Text { get; set; }

        public HttpPostedFileBase NewImageFile { get; set; }

        // Add other properties as needed
    }
    public class SearchResultViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ProfileImagePath { get; set; }

        // Add other properties as needed
    }
}
