using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimpleMessenger.Data.Models;
using System.Collections.Generic;

namespace SimpleMessenger.Data.Contexts
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            modelBuilder.Entity<Message>().HasMany(message => message.Recipients).WithMany(user => user.IncomingMessages).
                UsingEntity<Dictionary<string, object>>("MessageRecipients",
                    message => message.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.Cascade),
                    recipient => recipient.HasOne<Message>().WithMany().HasForeignKey("MessageId").OnDelete(DeleteBehavior.Restrict),
                    resultingEntity => resultingEntity.ToTable("MessageRecipients"));
        }

        public DbSet<Message> Messages { get; set; }
    }
}
