using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace EmailSender.Models.Domains.Repositories
{
    public class EmailMessageRepository
    {
        public List<EmailMessage> GetMessages(string userId)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.EmailMessages
                    .Include(x => x.EmailRecipient)
                    .Include(x => x.User)
                    .Where(x => x.UserId == userId)
                    .ToList();
            }
        }

        public void Add(EmailMessage emailMessage)
        {
            using (var context = new ApplicationDbContext())
            {
                context.EmailMessages.Add(emailMessage);
                context.SaveChanges();
            }
        }
    }
}