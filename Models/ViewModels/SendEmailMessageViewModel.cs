using EmailSender.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmailSender.Models.ViewModels
{
    public class SendEmailMessageViewModel
    {

        public string Heading { get; set; }
        public EmailMessage EmailMessage { get; set; }
        public List<EmailRecipient> EmailRecipients { get; set; }
        public List<EmailMessage> EmailMessages { get; set; }
    }
}