namespace BonusSystemApplication.DAL.Entities
{
    public class Definition
    {
        public long Id { get; set; }
        public Periods Period { get; set; }
        public int Year { get; set; } = DateTime.Now.Year;
        public bool IsWpmHox { get; set; }
        public long EmployeeId { get; set; }
        public User Employee { get; set; }
        public long? ManagerId { get; set; }
        public User? Manager { get; set; }
        public long? ApproverId { get; set; }
        public User? Approver { get; set; }
        public long? WorkprojectId { get; set; }
        public Workproject? Workproject { get; set; }

        public long FormId { get; set; }
        public Form Form { get; set; }
    }
}
