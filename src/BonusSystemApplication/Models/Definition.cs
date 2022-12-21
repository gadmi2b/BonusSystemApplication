using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BonusSystemApplication.Models
{
    public class Definition
    {
        public long Id { get; set; }
        public Periods Period { get; set; }
        public int Year { get; set; } = System.DateTime.Now.Year;
        public bool IsWpmHox { get; set; }
        public long EmployeeId { get; set; }
        [BindNever]
        public User Employee { get; set; }
        public long? ManagerId { get; set; }
        [BindNever]
        public User? Manager { get; set; }
        public long? ApproverId { get; set; }
        [BindNever]
        public User? Approver { get; set; }
        public long? WorkprojectId { get; set; }
        [BindNever]
        public Workproject? Workproject { get; set; }

        [BindNever]
        public Form Form { get; set; }
    }
}
