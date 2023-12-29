using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication7.Models
{
    public class MessageViewModel
    {
    }
    public class ChatViewModel
    {
        public List<chat> Messages { get; set; }
        public ApplicationUser OtherUser { get; set; }
        public ApplicationUser CurrentUser { get; set; }
    }

}