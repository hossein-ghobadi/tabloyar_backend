using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
        //DbSet<Category> Categories { get; set; }
        //DbSet<Content> Contents { get; set; }
        //DbSet<Comment> Comments { get; set; }
        //DbSet<SubComment> SubComments { get; set; }
        //DbSet<ClaimInfo> ClaimInfos { get; set; }
        //DbSet<ClaimCategoryInfo> ClaimCategories { get; set; }


        //DbSet<HomeSlider> HomeSliders { get; set; }
        //DbSet<ContactMessage> ContactMessages { get; set; }
        //DbSet<MainFactor> MainFactors { get; set; }
        //DbSet<ProductFactor> ProductFactors { get; set; }
        //DbSet<Accessory> Accessories { get; set; }
        ////DbSet<Service> Services { get; set; }

        //DbSet<PaymentReport> PaymentReports { get; set; }
        //DbSet<CheckPayment> CheckPayments { get; set; }


        DbSet<CityInfo> Cities { get; set; }
        //DbSet<Country> Countries { get; set; }
        //DbSet<ProductPriceDetail> ProductPriceDetails { get; set; }




        void MarkAsModified<T>(T entity) where T : class;
        void MarkPropertyAsModified<T, TProperty>(T entity, Expression<Func<T, TProperty>> property) where T : class;

        int SaveChanges(bool acceptAllChangesOnsuccess);
        int SaveChanges();
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellation = new CancellationToken());
        Task<int> SaveChangesAsync(CancellationToken cancellation = new CancellationToken());
        Task<IDbContextTransaction> BeginTransactionAsync();  // Add this method to the interface

    }
}
