using System;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WebApplication7.Models;
using System.Web.UI;
using System.Data.Entity;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using Microsoft.AspNet.SignalR;
using WebApplication7;

namespace YourNamespace.Controllers
{
    [RoutePrefix("Home")]
    public class HomeController : Controller
    {
        private IHubContext _hubContext;

        private ApplicationDbContext dbContext = new ApplicationDbContext();

        public HomeController()
        {
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
        }
        public ActionResult Index(string query)
        {
            // Create a new instance of the PostViewModel to store data for the view
            var viewModel = new PostViewModel();

            // Check if the user is authenticated
            if (User.Identity.IsAuthenticated)
            {
                // Get the current user's ID
                string currentUserId = User.Identity.GetUserId();

                // Fetch the current user's profile information from the database
                var user = dbContext.Users.FirstOrDefault(u => u.Id == currentUserId);

                if (user != null)
                {
                    // If the user is found, set ViewBag properties for use in the view
                    ViewBag.ProfileImagePath = user.ProfileImagePath;
                    ViewBag.UserName = user.UserName;
                }

                // Retrieve notifications for the current user
                viewModel.Notifications = dbContext.Notifications
                    .Where(n => n.UserId == currentUserId)
                    .OrderByDescending(n => n.Timestamp)
                    .Select(n => n.Message)
                    .ToList();

                // Fetch the current user's followers
                viewModel.Followers = dbContext.Users
                    .Where(u => u.Followers.Any(f => f.Id == currentUserId))
                    .ToList();
                viewModel.FollowersCount = viewModel.Followers.Count;

                // Fetch the current user's followings
                viewModel.Followings = dbContext.Users
                    .Where(u => u.Following.Any(f => f.Id == currentUserId))
                    .ToList();
                viewModel.FollowingCount = viewModel.Followings.Count;
            }

            // Retrieve regular posts from the database
            viewModel.Posts = dbContext.Posts
                .Include(p => p.User)
                .Include(p => p.Likes.Select(l => l.User))
                .OrderByDescending(p => p.PostedOn)
                .ToList();

            // Check if a search query is provided
            if (!string.IsNullOrWhiteSpace(query))
            {
                // If there is a search query, perform a search to find users based on the query
                var searchResults = dbContext.Users
                    .Where(u => u.UserName.Contains(query))
                    .Select(u => new SearchResultViewModel
                    {
                        UserId = u.Id,
                        UserName = u.UserName,
                        ProfileImagePath = u.ProfileImagePath,
                // Add other properties as needed
            })
                    .ToList();

                // Set the search results in the view model
                viewModel.SearchResults = searchResults;
            }

            // Return the view with the populated view model
            return View(viewModel);
        }


      


        [HttpPost]
        public ActionResult CreatePost(PostViewModel viewModel, HttpPostedFileBase imageFile, HttpPostedFileBase videoFile)
        {
            Post post = null;

            if (ModelState.IsValid)
            {
                string text = viewModel.NewPostText;
                string imagePath = null;
                string videoPath = null;

                // Check if an image file is uploaded
                if (imageFile != null)
                {
                    imagePath = SaveImage(imageFile);
                }

                // Check if a video file is uploaded
                if (videoFile != null)
                {
                    videoPath = SaveVideo(videoFile);
                }

                if (!string.IsNullOrWhiteSpace(text) || !string.IsNullOrWhiteSpace(imagePath) || !string.IsNullOrWhiteSpace(videoPath))
                {
                    post = new Post
                    {
                        Text = text,
                        PostedOn = DateTime.Now,
                        UserId = User.Identity.GetUserId(),
                        ImagePath = imagePath,
                        VideoPath = videoPath
                    };

                    dbContext.Posts.Add(post);
                    dbContext.SaveChanges();
                }
            }

            // Notify followers about the new post
            if (post != null)
            {
                NotifyFollowers(post.UserId, "New post added!");
            }

            return RedirectToAction("Index");
        }

        private string SaveVideo(HttpPostedFileBase videoFile)
        {
            string videoName = Path.GetFileName(videoFile.FileName);
            string videoPath = Path.Combine(Server.MapPath("~/Videos/PostVideos"), videoName);

            Directory.CreateDirectory(Server.MapPath("~/Videos/PostVideos"));

            videoFile.SaveAs(videoPath);

            return "~/Videos/PostVideos/" + videoName;
        }


        private void NotifyFollowers(string userId, string message)
        {
            var followers = dbContext.Users
                .Where(u => u.Following.Any(f => f.Id == userId))
                .Select(u => u.Id)
                .ToList();

            foreach (var followerId in followers)
            {
                _hubContext.Clients.User(followerId).notify(message);

                // Create a new notification record
                var notification = new Notification
                {
                    UserId = followerId,
                    Message = message,
                    Timestamp = DateTime.Now
                };

                dbContext.Notifications.Add(notification);
                dbContext.SaveChanges();
            }
        }

        private string SaveImage(HttpPostedFileBase imageFile)
        {
            string imageName = Path.GetFileName(imageFile.FileName);
            string imagePath = Path.Combine(Server.MapPath("~/Images/PostImages"), imageName);

            Directory.CreateDirectory(Server.MapPath("~/Images/PostImages"));

            imageFile.SaveAs(imagePath);

            return "~/Images/PostImages/" + imageName;
        }

        public ActionResult Like(int postId)
        {
            var userId = User.Identity.GetUserId();
            var like = new Like
            {
                PostId = postId,
                UserId = userId
            };

            dbContext.Likes.Add(like);
            dbContext.SaveChanges();

            // Return a JSON response indicating success or any additional data you want
            return Json(new { success = true });
        }

        public ActionResult Unlike(int postId)
        {
            var userId = User.Identity.GetUserId();
            var like = dbContext.Likes.FirstOrDefault(l => l.PostId == postId && l.UserId == userId);

            if (like != null)
            {
                dbContext.Likes.Remove(like);
                dbContext.SaveChanges();

                // Return a JSON response indicating success or any additional data you want
                return Json(new { success = true });
            }
            else
            {
                // Return a JSON response indicating failure or any additional data you want
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public ActionResult AddComment(int postId, string commentText)
        {
            try
            {
                // Get the current user
                var userId = User.Identity.GetUserId();

                // Create a new Comment object
                var comment = new Comment
                {
                    Text = commentText,
                    PostId = postId,
                    UserId = userId,
                    CommentedOn = DateTime.Now
                };

                // Add the comment to the database
                dbContext.Comments.Add(comment);
                dbContext.SaveChanges();

                // Return a JSON response indicating success
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return Json(new { success = false, error = ex.Message });
            }
        }




        [HttpGet]
        public JsonResult GetComments(int postId)
        {
            var comments = dbContext.Comments
                .Where(c => c.PostId == postId)
                .Include(c => c.User)
                .Select(c => new CommentViewModel
                {
                    UserId = c.User.Id,
                    UserName = c.User.UserName,
                    Text = c.Text
                    // Include other properties as needed
                })
                .ToList();

            return Json(comments, JsonRequestBehavior.AllowGet);
        }




        [System.Web.Mvc.Authorize]
        [HttpGet]
        public ActionResult EditPost(int postId)
        {
            var post = dbContext.Posts.Find(postId);

            if (post == null || post.UserId != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }

            var model = new EditPostViewModel
            {
                PostId = post.Id,
                Text = post.Text
            };

            return View(model);
        }

        

        [Route("Profile/Delete/{userId}")]
        public ActionResult DeletePost(int postId, string userId)
        {

            userId = User.Identity.GetUserId();
            if (userId == null)
            {
                // Handle the case when the user is not authenticated
                // You might want to redirect them to a login page or take appropriate action
                return Content("User Id Null");
            }

            var post = dbContext.Posts.FirstOrDefault(p => p.Id == postId && p.UserId == userId);

            if (post != null)
            {
                // Remove the post from the database
                dbContext.Posts.Remove(post);
                dbContext.SaveChanges();

                // Redirect to the user's profile page
                return RedirectToAction("UserProfile");
            }
            else
            {
                // Post not found, return HttpNotFound status
                return HttpNotFound();
            }

        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }


        [HttpGet]
        public ActionResult UserProfile(string userId)
        {
            var currentUserId = User.Identity.GetUserId();
            var user = dbContext.Users
                .Include(u => u.UserPosts)
                .Include(u => u.Followers)
                .Include(u => u.Following)
                .FirstOrDefault(u => u.Id == userId);

            if (user != null)
            {
                var profileModel = new ProfileViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    ProfileImagePath = user.ProfileImagePath,
                    Email = user.Email,
                    UserPosts = user.UserPosts.ToList(),
                    FollowersCount = user.Followers.Count,
                    FollowingCount = user.Following.Count,
                    IsFollowed = user.Followers.Any(f => f.Id == currentUserId),
                    Followers = user.Followers.ToList(),
                    Following = user.Following.ToList(),
                    RecentActivities = dbContext.Activities
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.Timestamp)
                .Take(5) // Limit to the latest 5 activities, adjust as needed
                .ToList()
                };

                ViewBag.ProfileImagePath = profileModel.ProfileImagePath;
                ViewBag.UserName = profileModel.UserName;

                // Record ViewProfile activity
                RecordViewProfileActivity(currentUserId, userId);

                Debug.WriteLine($"UserProfile action hit for userId: {userId}");

                return View("Profile", profileModel as object);
            }
            else
            {
                return HttpNotFound();
            }
        }
        private void RecordViewProfileActivity(string viewerUserId, string viewedUserId)
        {
            var activity = new Activity
            {
                UserId = viewerUserId,
                Timestamp = DateTime.Now,
                Type = ActivityType.ViewProfile,
                ViewedUserId = viewedUserId
            };

            dbContext.Activities.Add(activity);
            dbContext.SaveChanges();
        }

        [HttpGet]
        [Route("Home/EditProfile")]
        public ActionResult EditProfile()
        {
            // Retrieve the current user's information
            var userId = User.Identity.GetUserId();
            var user = dbContext.Users.FirstOrDefault(u => u.Id == userId);

            if (user != null)
            {
                var editProfileModel = new EditProfileViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    ProfileImagePath = user.ProfileImagePath
                };

                return View(editProfileModel);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [Route("Home/EditProfile")]
        public ActionResult EditProfile(ProfileViewModel model, HttpPostedFileBase newProfileImage)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the user from the database
                var user = dbContext.Users.FirstOrDefault(u => u.Id == model.UserId);

                if (user != null)
                {
                    // Update the username
                    user.UserName = model.UserName;

                    // Update the profile image if a new one is provided
                    if (newProfileImage != null)
                    {
                        string newImagePath = SaveImage(newProfileImage);
                        user.ProfileImagePath = newImagePath;
                    }

                    // Save changes to the database
                    dbContext.SaveChanges();

                    // Redirect to the user's profile page
                    return RedirectToAction("UserProfile", new { userId = model.UserId });
                }
                else
                {
                    return HttpNotFound();
                }
            }

            // If ModelState is not valid, return to the edit profile page with validation errors
            return View(model);
        }


        private void RecordEditProfileActivity(string userId)
        {
            var activity = new Activity
            {
                UserId = userId,
                Timestamp = DateTime.Now,
                Type = ActivityType.EditProfile
            };

            dbContext.Activities.Add(activity);
            dbContext.SaveChanges();
        }



        public ActionResult Follow(string userIdToFollow)
        {
            var userId = User.Identity.GetUserId();
            var userToFollow = dbContext.Users.FirstOrDefault(u => u.Id == userIdToFollow);

            if (userToFollow != null)
            {
                var currentUser = dbContext.Users
                    .Include(u => u.Following)
                    .FirstOrDefault(u => u.Id == userId);

                currentUser.Following.Add(userToFollow);
                dbContext.SaveChanges();
                userToFollow.Followers.Add(currentUser);
                dbContext.SaveChanges();

                return RedirectToAction("UserProfile", new { userId = userIdToFollow });
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult Unfollow(string userIdToUnfollow)
        {
            var userId = User.Identity.GetUserId();
            var userToUnfollow = dbContext.Users.FirstOrDefault(u => u.Id == userIdToUnfollow);

            if (userToUnfollow != null)
            {
                var currentUser = dbContext.Users
                    .Include(u => u.Following)
                    .FirstOrDefault(u => u.Id == userId);

                currentUser.Following.Remove(userToUnfollow);
                dbContext.SaveChanges();
                userToUnfollow.Followers.Remove(currentUser);
                dbContext.SaveChanges();

                return RedirectToAction("UserProfile", new { userId = User.Identity.GetUserId() });
            }
            else
            {
                return HttpNotFound();
            }
        }
    }
}