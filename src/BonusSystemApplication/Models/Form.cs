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
        public string? ObjectivesEmployeeSignature { get; set; }
        public bool IsObjectivesSignedByEmployee { get; set; }
        public bool IsObjectivesRejectedByEmployee { get; set; }
        public string? ObjectivesManagerSignature { get; set; }
        public bool IsObjectivesSignedByManager { get; set; }
        public string? ObjectivesApproverSignature { get; set; }
        public bool IsObjectivesSignedByApprover { get; set; }

        public bool IsResultsFreezed { get; set; }
        public string? ResultsEmployeeSignature { get; set; }
        public bool IsResultsSignedByEmployee { get; set; }
        public bool IsResultsRejectedByEmployee { get; set; }
        public string? ResultsManagerSignature { get; set; }
        public bool IsResultsSignedByManager { get; set; }
        public string? ResultsApproverSignature { get; set; }
        public bool IsResultsSignedByApprover { get; set; }

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
