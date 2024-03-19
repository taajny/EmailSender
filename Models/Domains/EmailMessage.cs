using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EmailSender.Models.Domains
{
    public class EmailMessage
    {
        public int Id { get; set; }

        [Display(Name = "Odbiorca")]
        public int EmailRecipientId { get; set; }
        [Display(Name = "Temat")]
        public string Subject { get; set; }

        [Display(Name = "Treść")]
        public string Body { get; set; }
        [Display(Name = "Data wysłania")]
        public DateTime DateOfSending { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }


        public EmailRecipient EmailRecipient { get; set; }

        public ApplicationUser User { get; set;  }

    }
}