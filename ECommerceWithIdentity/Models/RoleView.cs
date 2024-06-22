using System.ComponentModel.DataAnnotations;

namespace ECommerceWithIdentity.Models
{
    public class RoleView
    {
        [Required]
        [Display(Name = "Role")]
        public string? RoleName { get; set; }
    }
}
