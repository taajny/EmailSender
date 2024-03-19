using EmailSender.Models.Domains;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace EmailSender.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<EmailMessage> EmailMessages { get; set; }  
        public DbSet<EmailRecipient> EmailRecipients { get; set; }
         

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(x => x.EmailMessages)
                .WithRequired(x => x.User)
                .HasForeignKey(x => x.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(x => x.EmailRecipients)
                .WithRequired(x => x.User)
                .HasForeignKey(x => x.UserId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}