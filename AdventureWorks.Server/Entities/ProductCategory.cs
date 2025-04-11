using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AdventureWorks.Server.Entities
{
    [Table("ProductCategory", Schema = "Production")]
    public class ProductCategory : BaseEntity, IFromId<ProductCategory>
    {
        new public int Id { get => ProductCategoryID; }
        [Key]
        public int ProductCategoryID { get; set; }
        public string Name { get; set; } = string.Empty;


        public static ProductCategory FromId(int id)
        {
            return new ProductCategory { ProductCategoryID = id };
        }
    }
}
