using System.ComponentModel.DataAnnotations;

namespace Product_Catalog.Core.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductCode { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string ProductName_EN { get; set; } = string.Empty;

        [StringLength(200)]
        public string? ProductName_AR { get; set; }

        [StringLength(100)]
        public string SKU { get; set; } = string.Empty;

        [Required]
        [Range(1.0, double.MaxValue ) ]
        public decimal Price { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public bool IsDeleted { get; set; } = false;
    }
}
