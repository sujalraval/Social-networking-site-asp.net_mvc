using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication7.Models
{
    // Follow model
    public class Follow
    {
        [Key]
        public int Id { get; set; }

        // Foreign key to the user who is being followed
        public string TargetUserId { get; set; }

        // Navigation property for the user who is being followed
        public virtual ApplicationUser TargetUser { get; set; }

        // Foreign key to the user who is following
        public string FollowerUserId { get; set; }

        // Navigation property for the user who is following
        public virtual ApplicationUser FollowerUser { get; set; }
    }

}