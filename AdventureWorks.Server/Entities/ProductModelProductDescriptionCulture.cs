using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdventureWorks.Server.Entities
{
    [Table("ProductModelProductDescriptionCulture", Schema = "Production")]
    [PrimaryKey("ProductModelID", "CultureID", "ProductDescriptionID")]
    public class ProductModelProductDescriptionCulture
    {
        [Key]
        public int ProductModelID { get; set; }
        [Key]
        public int ProductDescriptionID { get; set; }
        [Key]
        public string CultureID { get; set; } = string.Empty;
        [ForeignKey("ProductModelID")]
        public ProductModel ProductModel { get; set; } = new ProductModel();
        [ForeignKey("ProductDescriptionID")]
        public ProductDescription ProductDescription { get; set; } = new ProductDescription();
    }
}
