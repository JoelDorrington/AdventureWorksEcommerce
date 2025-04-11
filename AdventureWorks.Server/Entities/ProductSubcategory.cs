using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AdventureWorks.Server.Entities
{
    [Table("ProductSubcategory", Schema = "Production")]
    public class ProductSubcategory : BaseEntity, IFromId<ProductSubcategory>
    {
        new public int Id { get => ProductSubcategoryID; }
        [Key]
        public int ProductSubcategoryID { get; set; }
        public int ProductCategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;


        public static ProductSubcategory FromId(int id)
        {
            return new ProductSubcategory { ProductSubcategoryID = id };
        }
    }
}
