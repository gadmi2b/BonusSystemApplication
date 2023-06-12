namespace BonusSystemApplication.Models.Forms.Edit
{
    public class ConclusionVM
    {
        public long Id { get; set; }
        public string? OverallKpi { get; set; }
        public bool IsProposalForBonusPayment { get; set; }
        public string? ManagerComment { get; set; }
        public string? EmployeeComment { get; set; }
        public string? OtherComment { get; set; }

        public ConclusionVM() { }
    }
}
