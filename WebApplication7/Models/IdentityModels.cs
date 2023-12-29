using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApplication7.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string ProfileImagePath { get; set; }
        public virtual ICollection<Post> UserPosts { get; set; }
        public virtual ICollection<ApplicationUser> Followers { get; set; } = new HashSet<ApplicationUser>();
        public virtual ICollection<ApplicationUser> Following { get; set; } = new HashSet<ApplicationUser>();
        public virtual ICollection<Activity> Activities { get; set; }
        // Navigation property for sent messages
        public virtual ICollection<chat> SentMessages { get; set; }

        // Navigation property for received messages
        public virtual ICollection<chat> ReceivedMessages { get; set; }


        public int FollowersCount => Followers.Count;
        public int FollowingCount => Following.Count;

        public bool IsFollowed(string userId)
        {
            return Followers.Any(f => f.Id == userId);
        }
        public bool IsCurrentUser(string currentUserId)
        {
            return Id == currentUserId;
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Activity> Activities { get; set; }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<chat> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public IEnumerable ApplicationUsers { get; internal set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the relationship between ApplicationUser and Activity
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Activities)
                .WithRequired(a => a.User)
                .HasForeignKey(a => a.UserId)
                .WillCascadeOnDelete(false); // Modify this based on your requirements
        }
    }
}
