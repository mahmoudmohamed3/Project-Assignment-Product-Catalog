using Microsoft.AspNetCore.Identity;

namespace Product_Catalog.Core.Models
{
    [Index(nameof(Email) , IsUnique = true)]
    [Index(nameof(UserName) , IsUnique = true)]
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(100)]
        public string FullName { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}