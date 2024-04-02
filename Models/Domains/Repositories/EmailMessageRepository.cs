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

        public void Delete(int id, string userId)
        {
            using (var context = new ApplicationDbContext())
            {
                var emailMessageToDelete = context.EmailMessages
                    .Include(x => x.EmailRecipient)
                    .Single(x => x.Id == id && x.UserId == userId);

                context.EmailMessages.Remove(emailMessageToDelete);
                context.SaveChanges();
            }
        }
    }
}