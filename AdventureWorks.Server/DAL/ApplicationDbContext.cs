using Microsoft.EntityFrameworkCore;
using AdventureWorks.Server.Entities;

namespace AdventureWorks.Server.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductModelProductDescriptionCulture>()
                .HasOne<ProductModel>()
                .WithMany(p => p.ProductModelProductDescriptionCultures)
                .HasForeignKey(p => p.ProductModelID);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.ProductModel)
                .WithMany()
                .HasForeignKey(p => p.ProductModelID)
                .IsRequired(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}

