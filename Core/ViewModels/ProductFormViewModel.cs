
namespace Product_Catalog.Core.ViewModels
{
    public class ProductFormViewModel
    {
        public int ProductId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Product Code")]
        public string ProductCode { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        [Display (Name ="Product Name")]
        public string ProductName_EN { get; set; } = string.Empty;

        [StringLength(200)]
        public string? ProductName_AR { get; set; }

        [StringLength(maximumLength: 100)]
        public string SKU { get; set; } = string.Empty;

        [Required]
        [Range(1.0, double.MaxValue, ErrorMessage = "Price must be more than 1.0")]
        public decimal Price { get; set; }
        
        [Display (Name ="Image")]
        public IFormFile? Image { get; set; }

        public string ImageUrl { get; set; } = String.Empty;

    }
}
