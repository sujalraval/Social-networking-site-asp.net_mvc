using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication7.Models
{
    public class ProfileViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ProfileImagePath { get; set; }
        public string Email { get; set; } // Add the Email property
        public List<Post> UserPosts { get; set; } // Add the UserPosts property
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
        public bool IsFollowed { get; set; }
        public string FollowerUserId { get; set; }
        public ApplicationDbContext DbContext { get; set; }
        public List<ApplicationUser> Followers { get; internal set; }
        public List<ApplicationUser> Following { get; internal set; }
        public List<ApplicationUser> SearchResults { get; set; }
        public List<Activity> RecentActivities { get; set; }




        // Add other user-related properties here
    }
    public class EditProfileViewModel
    {
        public string UserId { get; set; }

        [Required]
        public string UserName { get; set; }

        public HttpPostedFileBase NewProfileImage { get; set; }

        // Add both getter and setter for ProfileImagePath
        public string ProfileImagePath { get; set; }
    }


}