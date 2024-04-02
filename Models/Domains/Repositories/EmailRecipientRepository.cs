using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Data.Entity;

namespace EmailSender.Models.Domains.Repositories
{
    public class EmailRecipientRepository
    {
        public void Add(EmailRecipient emailRecipient)
        {
            using (var context = new ApplicationDbContext())
            {
                context.EmailRecipients.Add(emailRecipient);
                context.SaveChanges();
            }
        }

        public void Update(EmailRecipient emailRecipient, string userId)
        {
            using (var context = new ApplicationDbContext())
            {
                var emailRecipientToUpdate = context.EmailRecipients
                    .Single(x => x.Id == emailRecipient.Id && x.UserId == userId);

                emailRecipientToUpdate.Name = emailRecipient.Name;
                emailRecipientToUpdate.EmailAddress = emailRecipient.EmailAddress;
                
                context.SaveChanges();
            }
        }

        public EmailRecipient GetEmailRecipient(int id, string userId)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.EmailRecipients
                    .Include(x => x.User)
                    .Single(x => x.Id == id && x.UserId == userId);
            }
            
        }

        public List<EmailRecipient> GetEmailRecipients(string userId)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.EmailRecipients
                    .Include(x => x.User)
                    .Where(x => x.UserId == userId)
                    .ToList();
            }
        }

        public void Delete(int id, string userId)
        {
            using (var context = new ApplicationDbContext())
            {
                var emailRecipientToDelete = context.EmailRecipients
                    .Include(x => x.EmailMessages)
                    .Single(x => x.Id == id && x.UserId == userId);

                context.EmailRecipients.Remove(emailRecipientToDelete);
                context.SaveChanges();
            }
        }
    }
}