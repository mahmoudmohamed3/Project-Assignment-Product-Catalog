using ExpressiveAnnotations.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Product_Catalog.Core.ViewModels
{
    public class UserFormViewModel
    {
        public string? Id { get; set; }

        [MaxLength(100, ErrorMessage = "Full Name must be less than 100 characters")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = null!;

        [Display(Name = "Username")]
        [MaxLength(20, ErrorMessage = "Username must be less than 20 characters")]
        public string UserName { get; set; } = null!;

        [EmailAddress]
        [MaxLength(200, ErrorMessage = "Email must be less than 20 characters")]
        public string Email { get; set; } = null!;



        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare(otherProperty: "Password", ErrorMessage = "The password and confirmation password do not match.")]
        [RequiredIf("Id == null", ErrorMessage = "Please confirm your password")]
        public string? ConfirmPassword { get; set; } = null!;


        [Display(Name = "Roles")]
        public IList<string> SelectedRoles { get; set; } = new List<string>();

        public IEnumerable<SelectListItem>? Roles { get; set; }
    }
}