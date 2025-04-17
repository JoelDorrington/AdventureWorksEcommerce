using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdventureWorks.Server.Entities
{
    [Table("Product", Schema = "Production")]
    public class Product : BaseEntity, IFromId<Product>
    {
        public Product(int ProductID) {
            this.ProductID = ProductID;
        }
        public static Product FromId(int Id)
        {
            return new Product(Id);
        }

        
        [Key]
        public int ProductID { get; set; }
        new public int Id { get => ProductID; }
        public string Name { get; set; } = string.Empty;
        [NotMapped]
        public string? Description { get; set; }
        public decimal? ListPrice { get; set; }
        public string ThumbnailPhotoUrl { get; set; } = string.Empty;
        public int? ProductModelID { get; set; }
        [ForeignKey("ProductModelID")]
        public ProductModel? ProductModel { get; set; }
        public string? ProductNumber { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }

    }
}
