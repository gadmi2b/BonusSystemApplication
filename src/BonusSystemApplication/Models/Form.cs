namespace BonusSystemApplication.Models
{
    public class Form
    {
        public long Id { get; set; }
        public Periods Period { get; set; }
        public int Year { get; set; } = System.DateTime.Now.Year;
        public DateTime? LastSavedDateTime { get; set; }
        public string? LastSavedBy { get; set; }

        public string? OverallKpi { get; set; }
        public bool IsWpmHox { get; set; }
        public bool IsProposalForBonusPayment { get; set; }

        public string? ManagerComment { get; set; }
        public string? EmployeeComment { get; set; }
        public string? OtherComment { get; set; }

        public bool IsObjectivesFreezed { get; set; }
        public bool IsResultsFreezed { get; set; }

        public Signatures Signatures { get; set; }
        public IList<ObjectiveResult> ObjectivesResults { get; set; }
        public ICollection<LocalAccess> LocalAccesses { get; set; }

        public long EmployeeId { get; set; }
        public User Employee { get; set; }
        public long? ManagerId { get; set; }
        public User? Manager { get; set; }
        public long? ApproverId { get; set; }
        public User? Approver { get; set; }
        public long? WorkprojectId { get; set; }
        public Workproject? Workproject { get; set; }
    }
}
