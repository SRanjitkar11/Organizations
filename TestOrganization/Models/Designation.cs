using System.ComponentModel.DataAnnotations;

namespace TestOrganization.Models
{
    public class Designation
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public string CallName { get; set; }
    }
}
