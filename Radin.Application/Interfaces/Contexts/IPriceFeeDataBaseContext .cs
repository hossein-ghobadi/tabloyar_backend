//using Microsoft.EntityFrameworkCore;
//using Radin.Domain.Entities.Claim;
//using Radin.Domain.Entities.ClaimsInfo;
//using Radin.Domain.Entities.Comments;
//using Radin.Domain.Entities.ContactUs;
//using Radin.Domain.Entities.Contents;
//using Radin.Domain.Entities.HomePage;
//using Radin.Domain.Entities.Ideas;
//using Radin.Domain.Entities.Products;
//using Radin.Domain.Entities.Products.Aditional;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radin.Application.Interfaces.Contexts
//{
//    public interface IPriceFeeDataBaseContext
//    {
       
//        DbSet<Smd> Smds { get; set; }
//        DbSet<Crystal> Crystals { get; set; }
//        DbSet<EdgeProperty> EdgeProperties { get; set; }
//        DbSet<EdgePunch> EdgePunchs { get; set; }
//        DbSet<Glue> Glues { get; set; }
//        DbSet<Material> Materials { get; set; }
//        DbSet<Power> Powers { get; set; }
//        DbSet<Punch> Punchs { get; set; }
//         DbSet<ColorCost> ColorCosts { get; set; }
//        DbSet<Title> Titles { get; set; }
//         DbSet<Margin> Margins { get; set; }
//        DbSet<QualityDegree> QualityDegrees { get; set; }
//        DbSet<MaterialEdgeColor> MaterialEdgeColors { get; set; }
//        DbSet<MaterialColor> MaterialColors { get; set; }
//        DbSet<SecondLayerMaterial> SecondLayerMaterials { get; set; }

//        DbSet<MaterialEdgeSize> MaterialEdgeSizes { get; set; }
        



//        int SaveChanges(bool acceptAllChangesOnsuccess);
//        int SaveChanges();
//        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellation = new CancellationToken());
//        Task<int> SaveChangesAsync(CancellationToken cancellation = new CancellationToken());
//    }
//}
