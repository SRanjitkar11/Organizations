using System.ComponentModel.DataAnnotations;

namespace TestOrganization.Models.DTO
{
    public class EmployeeDTO
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Address { get; set; }

        public string OrganizationId { get; set; }

        public string Designation { get; set; }
    }
}
