using System.ComponentModel.DataAnnotations;

namespace TestOrganization.Models
{
    public class Employee
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Address { get; set; }

        public string Designation { get; set; }

        public string OrganizationId { get; set; }

        public string? ReportsTo { get; set; }
    }
}