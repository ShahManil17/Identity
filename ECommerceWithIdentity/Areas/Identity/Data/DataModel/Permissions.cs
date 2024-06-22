using System.ComponentModel.DataAnnotations;

namespace ECommerceWithIdentity.Areas.Identity.Data.DataModel
{
    public class Permissions
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }
    }
}
