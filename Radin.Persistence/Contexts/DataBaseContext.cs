using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Radin.Application.Interfaces.Contexts;
using Radin.Common.Dto;
using Radin.Domain.Entities.Others;
//using Radin.Domain.Entities.Claim;
//using Radin.Domain.Entities.ClaimsInfo;
//using Radin.Domain.Entities.Comments;
//using Radin.Domain.Entities.ContactUs;
//using Radin.Domain.Entities.Contents;
//using Radin.Domain.Entities.Factors;
//using Radin.Domain.Entities.HomePage;
//using Radin.Domain.Entities.Others;
//using Radin.Domain.Entities.Products;
//using Radin.Domain.Entities.Products.Aditional;

using Radin.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Contexts
{
    public class DataBaseContext : DbContext, IDataBaseContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
        }
        //public DbSet<Content> Contents { get; set; }
        //public DbSet<Category> Categories { get; set; }
        //public DbSet<Comment> Comments { get; set; }
        //public DbSet<SubComment> SubComments { get; set; }
        //public DbSet<ClaimInfo> ClaimInfos { get; set; }
        //public DbSet<ClaimCategoryInfo> ClaimCategories { get; set; }

        //public DbSet<HomeSlider> HomeSliders { get; set; }
        //public DbSet<ContactMessage> ContactMessages { get; set; }


        //public DbSet<MainFactor> MainFactors { get; set; }

        //public DbSet<ProductFactor> ProductFactors { get; set; }


        //public DbSet<Accessory> Accessories { get; set; }
        //public DbSet<Service> Services { get; set; }
        //public DbSet<PaymentReport> PaymentReports { get; set; }
        //public DbSet<CheckPayment> CheckPayments { get; set; }
        public DbSet<CityInfo> Cities { get; set; }
        //public DbSet<Country> Countries { get; set; }

        //public DbSet<ProductPriceDetail> ProductPriceDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //modelBuilder.Entity<Category>().HasIndex(u => u.CategoryTitle).IsUnique();

            //modelBuilder.Entity<MainFactor>().Property(p => p.Id).UseIdentityColumn(10001,1);

            //modelBuilder.Entity<ClaimCategoryInfo>().HasData(
            //    new ClaimCategoryInfo { Id = 1, CategoryName = ClaimCategoryConstant.FixCategory, Description = "سایر" }
            //    );




           // modelBuilder.Entity<MainFactor>()
           //.HasMany(f => f.ProductFactors)
           //.WithOne(s => s.MainFactors)
           //.HasForeignKey(s => s.FactorID)
           //.OnDelete(DeleteBehavior.Cascade);  // Cascade delete

            //modelBuilder.Entity<MainFactor>()
            //.HasOne(mf => mf.PaymentReports) // MainFactor has one PaymentReport
            //.WithOne(pr => pr.MainFactors) // PaymentReport has one MainFactor
            //.HasForeignKey<PaymentReport>(pr => pr.FactorId) // Foreign key in PaymentReport
            //.OnDelete(DeleteBehavior.Cascade); // Cascade delete

            //modelBuilder.Entity<PaymentReport>()
            //    .HasMany(s => s.CheckPayments)
            //    .WithOne(p => p.PaymentReports)
            //    .HasForeignKey(p => p.PaymentId)
            //    .OnDelete(DeleteBehavior.Cascade);  // Cascade delet


            //modelBuilder.Entity<ProductFactor>()
            //    .HasMany(s => s.ProductPriceDetails)
            //    .WithOne(p => p.ProductFactors)
            //    .HasForeignKey(p => p.ProductId)
            //    .OnDelete(DeleteBehavior.Cascade);  // Cascade delete

            // Other configurations...

            base.OnModelCreating(modelBuilder);
        }
        public void MarkAsModified<T>(T entity) where T : class
        {
            Entry(entity).State = EntityState.Modified;
        }
        public void MarkPropertyAsModified<T, TProperty>(T entity, Expression<Func<T, TProperty>> property) where T : class
        {
            Entry(entity).Property(property).IsModified = true;
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await Database.BeginTransactionAsync();  // Expose this method from the DbContext
        }


    }
}
