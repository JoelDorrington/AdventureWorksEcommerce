using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdventureWorks.Server.Entities
{
    [Table("ProductDescription", Schema = "Production")]
    public class ProductDescription
    {
        [Key]
        public int ProductDescriptionID { get; set; }
        public string Description { get; set; } = string.Empty;
        public string rowguid { get; set; } = string.Empty;
        public DateTime ModifiedDate { get; set; }
    }
}
