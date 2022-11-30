namespace BonusSystemApplication.Models
{
    public class ForObjectives
    {
        public string? EmployeeSignature { get; set; }
        public bool IsSignedByEmployee { get; set; }
        public bool IsRejectedByEmployee { get; set; }
        public string? ManagerSignature { get; set; }
        public bool IsSignedByManager { get; set; }
        public string? ApproverSignature { get; set; }
        public bool IsSignedByApprover { get; set; }
    }
}
