using System.ComponentModel.DataAnnotations;

namespace DataModel.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required]
        [MaxLength(40)]
        public string Name { get; set; }
        public double Price { get; set; }
    }
}
