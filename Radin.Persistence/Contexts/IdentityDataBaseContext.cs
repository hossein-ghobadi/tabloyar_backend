using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Radin.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Persistence.Contexts
{
    public class IdentityDataBaseContext : IdentityDbContext<User>
    {
        public IdentityDataBaseContext(DbContextOptions<IdentityDataBaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(l => new { l.LoginProvider, l.ProviderKey });
            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(r => new { r.UserId, r.RoleId });
            modelBuilder.Entity<IdentityUserToken<string>>().HasKey(t => new { t.UserId, t.LoginProvider, t.Name });


            // Configure unique index for the PhoneNumber field in the AspNetUsers table
            //modelBuilder.Entity<User>().HasIndex(u => u.PhoneNumber).IsUnique();
            //modelBuilder.Entity<IdentityRole>().HasData(new {Id="fuofhaegaog", Name = "ADMIN" }, new {Id="hdGDUFHYGuf", Name = "USER" });
        }
    }
}
