using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WebApplication7.Models;

public class MessageController : Controller
{
    private ApplicationDbContext db = new ApplicationDbContext();

    // GET: Messages
    public ActionResult Index()
    {
        var messages = db.Messages.ToList();
        return View(messages);
    }

    public ActionResult Chat(string userId)
    {
        var currentUserId = User.Identity.GetUserId();
        var currentUser = db.Users.Find(currentUserId);
        var otherUser = db.Users.Find(userId);

        var messages = db.Messages
            .Where(m =>
                (m.SenderID == currentUserId && m.ReceiverID == otherUser.Id) ||
                (m.SenderID == otherUser.Id && m.ReceiverID == currentUserId))
            .ToList();

        var viewModel = new ChatViewModel
        {
            Messages = messages,
            OtherUser = otherUser,
            CurrentUser = currentUser
        };

        return View(viewModel);
    }

    [HttpPost]
    public ActionResult UploadImage(HttpPostedFileBase file)
    {
        if (file != null && file.ContentLength > 0)
        {
            // Save the image to the "Images/Chat" folder
            var fileName = Path.GetFileName(file.FileName);
            var path = Path.Combine(Server.MapPath("~/Images/Chat"), fileName);
            file.SaveAs(path);
            // Return the file name to the client
            return Json(new { FileName = fileName });
        }
        return HttpNotFound();
    }

    // Other actions...

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            db.Dispose();
        }
        base.Dispose(disposing);
    }
}
