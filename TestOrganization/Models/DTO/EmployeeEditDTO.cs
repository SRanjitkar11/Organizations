using System.ComponentModel.DataAnnotations;

namespace TestOrganization.Models.DTO
{
    public class EmployeeEditDTO
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Address { get; set; }

        public string Designation { get; set; }

        [Display(Name = "Reports To")]
        public string? ReportsTo { get; set; }
    }
}
