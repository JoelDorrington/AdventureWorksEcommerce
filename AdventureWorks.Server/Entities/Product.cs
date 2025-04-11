using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdventureWorks.Server.Entities
{
    [Table("Product", Schema = "Production")]
    public class Product : BaseEntity, IFromId<Product>
    {
        public Product(int Id) {
            this.ProductID = Id;
        }
        public static Product FromId(int Id)
        {
            return new Product(Id);
        }

        
        [Key]
        public int ProductID { get; set; }
        new public int Id { get => ProductID; }
        public string Name { get; set; } = string.Empty;
        public decimal? ListPrice { get; set; }
        public string ThumbnailPhotoUrl { get; set; } = string.Empty;

    }
}
