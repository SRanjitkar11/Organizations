using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestOrganization.Models
{
    public class OrganizationUser
    {
        [Key]
        [ForeignKey("UserId")]
        public string UserId { get; set; }

        [Key]
        [ForeignKey("Id")]
        public string OrganizationId { get; set; }
    }
}
