using System.ComponentModel.DataAnnotations;

namespace TestOrganization.Models
{
    public class Organization
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        [StringLength(2, MinimumLength = 2)]
        public string Code { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public ICollection<ApplicationUser>? ApplicationUser { get; set; }
    }
}
