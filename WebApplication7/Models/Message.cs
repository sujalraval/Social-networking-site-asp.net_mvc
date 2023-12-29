using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebApplication7.Models
{


    public class chat
    {
        [Key]
        public int MessageID { get; set; }

        [Required]
        public string Content { get; set; }

        public string ImagePath { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        // Foreign key relationship with ApplicationUser
        [Required]
        [ForeignKey("Sender")]
        public string SenderID { get; set; }

        [Required]
        [ForeignKey("Receiver")]
        public string ReceiverID { get; set; }

        // Navigation properties
        public virtual ApplicationUser Sender { get; set; }

        public virtual ApplicationUser Receiver { get; set; }
    }

}