using Microsoft.EntityFrameworkCore;
using Radin.Application.Interfaces.Contexts;
using Radin.Domain.Entities.Claim;
using Radin.Domain.Entities.ClaimsInfo;
using Radin.Domain.Entities.Comments;
using Radin.Domain.Entities.ContactUs;
using Radin.Domain.Entities.Contents;
using Radin.Domain.Entities.HomePage;
using Radin.Domain.Entities.Ideas;
using Radin.Domain.Entities.Products;
using Radin.Domain.Entities.Products.Aditional;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Contexts
{
    public class PriceFeeDataBaseContext : DbContext, IPriceFeeDataBaseContext
    {
        public PriceFeeDataBaseContext(DbContextOptions<PriceFeeDataBaseContext> options) : base(options)
        {
        }
        


        public DbSet<Smd> Smds { get; set; }
        public DbSet<Crystal> Crystals { get; set; }
        public DbSet<EdgeProperty> EdgeProperties { get; set; }
        public DbSet<EdgePunch> EdgePunchs { get; set; }
        public DbSet<Glue> Glues { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Power> Powers { get; set; }
        public DbSet<Punch> Punchs { get; set; }
        public DbSet<ColorCost> ColorCosts { get; set; }
        public DbSet<Title> Titles {  get; set; }
        
        public DbSet<Margin> Margins { get; set; }
        public DbSet<QualityDegree> QualityDegrees { get; set; }
        public DbSet<MaterialEdgeColor> MaterialEdgeColors { get; set; }

        public DbSet<MaterialEdgeSize> MaterialEdgeSizes { get; set; }
        public DbSet<SecondLayerMaterial> SecondLayerMaterials { get; set; }
        public DbSet<MaterialColor> MaterialColors { get; set; }
       





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


        }



    }
}
