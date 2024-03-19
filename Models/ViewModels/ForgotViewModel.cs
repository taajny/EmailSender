using System.ComponentModel.DataAnnotations;

namespace EmailSender.Models
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Adres e-mail")]
        public string Email { get; set; }
    }
}
