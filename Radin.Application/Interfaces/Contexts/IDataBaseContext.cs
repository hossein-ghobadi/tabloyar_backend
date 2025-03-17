using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Interfaces.Contexts
{
    public interface IDataBaseContext
    {
        DbSet<T> Set<T>() where T : class;
        DbSet<Category> Categories { get; set; }
        DbSet<Content> Contents { get; set; }
        DbSet<Comment> Comments { get; set; }
        DbSet<SubComment> SubComments { get; set; }
        DbSet<ClaimInfo> ClaimInfos { get; set; }
        DbSet<ClaimCategoryInfo> ClaimCategories { get; set; }


        DbSet<HomeSlider> HomeSliders { get; set; }
        DbSet<ContactMessage> ContactMessages { get; set; }

        DbSet<Idea> Ideas { get; set; }
        DbSet<IdeaCategory> IdeaCategories { get; set; }
        DbSet<IdeaComment> IdeaComments { get; set; }
        DbSet<IdeaSubComment> IdeaSubComments { get; set; }
        DbSet<IdeaRank> IdeaRanks { get; set; }

        DbSet<Sample> Samples { get; set; }
        DbSet<SampleCategory> SampleCategories { get; set; }
        DbSet<SampleComment> SampleComments { get; set; }
        DbSet<SampleSubComment> SampleSubComments { get; set; }
        DbSet<SampleRank> SampleRanks { get; set; }

        DbSet<BranchINFO> BranchINFOs { get; set; }
        DbSet<BranchRegister> BranchRegisters { get; set; }

        DbSet<ContactTypeInfo> ContactTypeInfo { get; set; }
        DbSet<CustomerInfo> CustomerInfo { get; set; }
        DbSet<MainFactor> MainFactors { get; set; }
        DbSet<ProductDesign> ProductDesigns { get; set; }
        DbSet<ProductFactor> ProductFactors { get; set; }
        DbSet<StatusReasons> StatusReasons { get; set; }
        DbSet<SubFactor> SubFactors { get; set; }
        DbSet<Accessory> Accessories { get; set; }
        DbSet<Service> Services { get; set; }
        DbSet<FactorContract> FactorContracts { get; set; }

        DbSet<PaymentReport> PaymentReports { get; set; }
        DbSet<CheckPayment> CheckPayments { get; set; }
        DbSet<FactorComplementaryType> FactorComplementaryTypes { get; set; }
        DbSet<FactorProductComplementary> FactorProductComplementaries { get; set; }



        DbSet<Genders> Genders { get; set; }    
        DbSet<Acquaintance> acquaintances { get; set; }
        DbSet<MarketOrient> marketOrients { get; set; }
        DbSet<JobCategoryInfo> JobCategoryInfo { get; set; }
        DbSet<AgeCategories> AgeCategories { get; set; }
        DbSet<CustomerConnection> CustomerConnections { get; set; }

        DbSet<CityInfo> Cities { get; set; }
        DbSet<Country> Countries { get; set; }

        DbSet<CharacterTypeDetails> CharacterTypeDetails { get; set; }
        DbSet<PersonalityCharacterType> PersonalityCharacterType { get; set; }
        DbSet<MonthlyTarget> MonthlyTargets { get; set; }
        DbSet<ProxyNotification> ProxyNotifications { get; set; }
        DbSet<ProductPriceDetail> ProductPriceDetails { get; set; }




        void MarkAsModified<T>(T entity) where T : class;
        void MarkPropertyAsModified<T, TProperty>(T entity, Expression<Func<T, TProperty>> property) where T : class;

        int SaveChanges(bool acceptAllChangesOnsuccess);
        int SaveChanges();
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellation = new CancellationToken());
        Task<int> SaveChangesAsync(CancellationToken cancellation = new CancellationToken());
        Task<IDbContextTransaction> BeginTransactionAsync();  // Add this method to the interface

    }
}
