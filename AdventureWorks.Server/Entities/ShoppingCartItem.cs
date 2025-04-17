using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdventureWorks.Server.Entities
{
    [Table("ShoppingCartItem", Schema = "Sales")]
    public class ShoppingCartItem
    {
        [Key]
        public int ShoppingCartItemId { get; set; }
        [Required]
        public string ShoppingCartId { get; set; } = null!;
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int ProductID { get; set; }
        [ForeignKey("ProductID")]
        public Product Product { get; set; } = null!;
        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        public DateTime ModifiedDate { get; set; }
    }
}
