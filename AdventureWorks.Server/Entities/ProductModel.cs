using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdventureWorks.Server.Entities
{
    [Table("ProductModel", Schema = "Production")]
    public class ProductModel
    {
        [Key]
        public int ProductModelID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CatalogDescription { get; set; } = string.Empty;
        public string Instructions { get; set; } = string.Empty;
        [Required]
        public string rowguid { get; set; } = string.Empty;
        public DateTime ModifiedDate { get; set; }
        public List<ProductModelProductDescriptionCulture> ProductModelProductDescriptionCultures { 
            get; 
            set; } = new List<ProductModelProductDescriptionCulture>();
    }
}
