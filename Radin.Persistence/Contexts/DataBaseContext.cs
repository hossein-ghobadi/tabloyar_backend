using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Radin.Application.Interfaces.Contexts;
using Radin.Common.Dto;
using Radin.Domain.Entities.Branches;
using Radin.Domain.Entities.Claim;
using Radin.Domain.Entities.ClaimsInfo;
using Radin.Domain.Entities.Comments;
using Radin.Domain.Entities.ContactUs;
using Radin.Domain.Entities.Contents;
using Radin.Domain.Entities.Customers;
using Radin.Domain.Entities.Factors;
using Radin.Domain.Entities.HomePage;
using Radin.Domain.Entities.Ideas;
using Radin.Domain.Entities.Message;
using Radin.Domain.Entities.OKR;
using Radin.Domain.Entities.Others;
using Radin.Domain.Entities.Products;
using Radin.Domain.Entities.Products.Aditional;
using Radin.Domain.Entities.Samples;
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
        public DbSet<Content> Contents { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<SubComment> SubComments { get; set; }
        public DbSet<ClaimInfo> ClaimInfos { get; set; }
        public DbSet<ClaimCategoryInfo> ClaimCategories { get; set; }

        public DbSet<HomeSlider> HomeSliders { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }


        public DbSet<Idea> Ideas { get; set; }
        public DbSet<IdeaCategory> IdeaCategories { get; set; }
        public DbSet<IdeaComment> IdeaComments { get; set; }
        public DbSet<IdeaSubComment> IdeaSubComments { get; set; }
        public DbSet<IdeaRank> IdeaRanks { get; set; }


        public DbSet<Sample> Samples { get; set; }
        public DbSet<SampleCategory> SampleCategories { get; set; }
        public DbSet<SampleComment> SampleComments { get; set; }
        public DbSet<SampleSubComment> SampleSubComments { get; set; }
        public DbSet<SampleRank> SampleRanks { get; set; }

        public DbSet<BranchINFO> BranchINFOs { get; set; }
        public DbSet<BranchRegister> BranchRegisters { get; set; }


        public DbSet<ContactTypeInfo> ContactTypeInfo { get; set; }
        public DbSet<CustomerInfo> CustomerInfo { get; set; }
        public DbSet<MainFactor> MainFactors { get; set; }
        public DbSet<ProductDesign> ProductDesigns { get; set; }
        public DbSet<ProductFactor> ProductFactors { get; set; }
        public DbSet<StatusReasons> StatusReasons { get; set; }
        public DbSet<SubFactor> SubFactors { get; set; }
        public DbSet<CustomerConnection> CustomerConnections { get; set; }

        public DbSet<Accessory> Accessories { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<PaymentReport> PaymentReports { get; set; }
        public DbSet<CheckPayment> CheckPayments { get; set; }
        public DbSet<FactorComplementaryType> FactorComplementaryTypes { get; set; }
        public DbSet<FactorProductComplementary> FactorProductComplementaries { get; set; }
        public DbSet<FactorContract> FactorContracts { get; set; }



        public DbSet<Genders> Genders { get; set; }
        public DbSet<Acquaintance> acquaintances { get; set; }
        public DbSet<MarketOrient> marketOrients { get; set; }
        public DbSet<JobCategoryInfo> JobCategoryInfo { get; set; }
        public DbSet<AgeCategories> AgeCategories { get; set; }
        public DbSet<CityInfo> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<CharacterTypeDetails> CharacterTypeDetails { get; set; }
        public DbSet<PersonalityCharacterType> PersonalityCharacterType { get; set; }

        public DbSet<MonthlyTarget> MonthlyTargets { get; set; }
        public DbSet<ProxyNotification> ProxyNotifications { get; set; }
        public DbSet<ProductPriceDetail> ProductPriceDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Category>().HasIndex(u => u.CategoryTitle).IsUnique();

            modelBuilder.Entity<Idea>()
             .HasKey(i => i.Id);
            modelBuilder.Entity<Sample>()
            .HasKey(i => i.Id);

            modelBuilder.Entity<Idea>()
                .HasMany(i => i.IdeaRanks)
                .WithOne(ir => ir.Idea)
                .HasForeignKey(ir => ir.IdeaId);

            modelBuilder.Entity<Sample>()
                .HasMany(i => i.SampleRanks)
                .WithOne(ir => ir.Sample)
                .HasForeignKey(ir => ir.SampleId);

            modelBuilder.Entity<IdeaRank>()
                .HasKey(ir => ir.Id);
            modelBuilder.Entity<SampleRank>()
               .HasKey(ir => ir.Id);

            modelBuilder.Entity<IdeaRank>()
                .Property(ir => ir.UserId)
                .IsRequired();

            modelBuilder.Entity<SampleRank>()
                .Property(ir => ir.UserId)
                .IsRequired();




            //modelBuilder.Entity<ContactInfo>().ToTable("ContactInfo", "dbo");


            modelBuilder.Entity<MainFactor>().Property(p => p.Id).UseIdentityColumn(10001,1);
            modelBuilder.Entity<CustomerInfo>().Property(p => p.Id).UseIdentityColumn(12201, 1);

            modelBuilder.Entity<ClaimCategoryInfo>().HasData(
                new ClaimCategoryInfo { Id = 1, CategoryName = ClaimCategoryConstant.FixCategory, Description = "سایر" }
                );

            modelBuilder.Entity<Genders>().HasData(
                new Genders { Id = 1, type = "مذکر" },
                new Genders { Id = 2, type = "مونث" }
                );

            modelBuilder.Entity<ContactTypeInfo>().HasData(
               new ContactTypeInfo { Id = 1, type = "تماس تلفنی" },
               new ContactTypeInfo { Id = 2, type = "فضای مجازی" },
               new ContactTypeInfo { Id = 3, type = "حضوری" }
               );

            modelBuilder.Entity<Acquaintance>().HasData(
                new Acquaintance {  Id = 1,type = "معرفی توسط دیگران" },
                new Acquaintance { Id = 2, type = "شبکه های اجتماعی" },
                new Acquaintance { Id = 3, type = "گذری" },
                new Acquaintance { Id = 4, type = "بازاریابی حضوری" },
                new Acquaintance { Id = 5, type = "بازاریابی تلفنی" },
                new Acquaintance { Id = 6, type = "سایت" },
                new Acquaintance { Id = 7, type = "شیپور و دیوار" },
                new Acquaintance { Id = 8, type = "تبلیغات محیطی" },
                new Acquaintance { Id = 9, type = "نمایشگاه" },
                new Acquaintance { Id = 10, type = "پیامک" },
                new Acquaintance { Id = 11, type = "بازاریابی 360 " },
                new Acquaintance { Id = 12,type = "تبلیغات کاغذی " }
                
                );
            modelBuilder.Entity<MarketOrient>().HasData(
                new MarketOrient { Id = 1, type ="صنایع و کسب و کارهای خرد"},
                new MarketOrient { Id = 2, type = "صنایع کلان" },
                new MarketOrient { Id = 3, type = "سازمان ها" },
                new MarketOrient { Id = 4, type = "واسطه ها" }
                );
            modelBuilder.Entity<AgeCategories>().HasData(
                new AgeCategories { Id = 1, category = "کمتر از 18 سال"},
                new AgeCategories { Id = 2, category = "بین 18 الی 24 سال" },
                new AgeCategories { Id = 3, category = "بین 25 الی 34 سال" },
                new AgeCategories { Id = 4, category = "بین 35 الی 44 سال" },
                new AgeCategories { Id = 5, category = "بین 45 الی 54 سال" },
                new AgeCategories { Id = 6, category = "بالای 54 سال" }
                );

            modelBuilder.Entity<PersonalityCharacterType>().HasData(
                new PersonalityCharacterType{ Id = 1, Type = "D"},
                new PersonalityCharacterType { Id = 2, Type = "I" },
                new PersonalityCharacterType { Id = 3, Type = "S" },
                new PersonalityCharacterType { Id = 4, Type = "C" },
                new PersonalityCharacterType { Id = 5, Type = "DI" },
                new PersonalityCharacterType { Id = 6, Type = "DC" },
                new PersonalityCharacterType { Id = 7, Type = "IS" },
                new PersonalityCharacterType { Id = 8, Type = "SC" },
                new PersonalityCharacterType { Id = 9, Type = "DIS" },
                new PersonalityCharacterType { Id = 10, Type = "ISC" },
                new PersonalityCharacterType { Id = 11, Type = "SCD" },
                new PersonalityCharacterType { Id = 12, Type = "CDI" },
                new PersonalityCharacterType { Id = 13, Type = "N" }
                );
            

            modelBuilder.Entity<CharacterTypeDetails>()
            .HasOne(ctd => ctd.Customer)
            .WithOne(c => c.CharacterTypeDetails)
            .HasForeignKey<CharacterTypeDetails>(ctd => ctd.CustomerID)
            .OnDelete(DeleteBehavior.Cascade); // Optional, defines delete behavior



            modelBuilder.Entity<MainFactor>()
           .HasMany(f => f.SubFactors)
           .WithOne(s => s.MainFactors)
           .HasForeignKey(s => s.FactorID)
           .OnDelete(DeleteBehavior.Cascade);  // Cascade delete
            modelBuilder.Entity<MainFactor>()
           .HasMany(f => f.CustomerConnections)
           .WithOne(s => s.MainFactors)
           .HasForeignKey(s => s.FactorID)
           .OnDelete(DeleteBehavior.Cascade);  // Cascade delete

            modelBuilder.Entity<MainFactor>()
            .HasOne(mf => mf.PaymentReports) // MainFactor has one PaymentReport
            .WithOne(pr => pr.MainFactors) // PaymentReport has one MainFactor
            .HasForeignKey<PaymentReport>(pr => pr.FactorId) // Foreign key in PaymentReport
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete

            modelBuilder.Entity<PaymentReport>()
                .HasMany(s => s.CheckPayments)
                .WithOne(p => p.PaymentReports)
                .HasForeignKey(p => p.PaymentId)
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete
           
            // Configure SubFactor -> Product relationship
            modelBuilder.Entity<SubFactor>()
                .HasMany(s => s.ProductFactors)
                .WithOne(p => p.SubFactor)
                .HasForeignKey(p => p.SubFactorID)
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete





            modelBuilder.Entity<ProductFactor>()
                .HasMany(s => s.ProductPriceDetails)
                .WithOne(p => p.ProductFactors)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete

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
