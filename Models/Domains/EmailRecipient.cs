
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmailSender.Models.Domains
{
    public class EmailRecipient
    {
        public EmailRecipient()
        {
            EmailMessages = new Collection<EmailMessage>();
        }
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nazwa użytkownika")]
        public string Name { get; set; }
        [Required]
        [Display(Name ="Adres e-mail")]
        public string EmailAddress { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }

        public ICollection<EmailMessage> EmailMessages { get; set; }
        public ApplicationUser User { get; set; }
    }
}