using System.ComponentModel.DataAnnotations;

namespace TestOrganization.Models
{
    public class LeaveRequest
    {
        [Key]
        public string EmployeeId { get; set; }

        public String Message { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Leave Date")]
        public DateTime LeaveDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Return Date")]
        public DateTime ReturnDate { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:%d}", ApplyFormatInEditMode = true)]
        public TimeSpan LeaveDay
        {
            get { return ReturnDate - LeaveDate; }
        }

        public contract Status { get; set; }
    }

    public enum contract
    {
        Pending,
        Approved,
        Rejected
    }
}
