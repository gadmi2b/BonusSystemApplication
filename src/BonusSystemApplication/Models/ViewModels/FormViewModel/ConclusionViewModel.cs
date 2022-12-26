namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    public class ConclusionViewModel
    {
        public string? OverallKpi { get; set; }
        public bool IsProposalForBonusPayment { get; set; } = false;
        public string? ManagerComment { get; set; }
        public string? EmployeeComment { get; set; }
        public string? OtherComment { get; set; }

        public ConclusionViewModel() { }
        public ConclusionViewModel(Conclusion source)
        {
            OverallKpi = source.OverallKpi;
            IsProposalForBonusPayment = source.IsProposalForBonusPayment;
            ManagerComment = source.ManagerComment;
            EmployeeComment = source.EmployeeComment;
            OtherComment = source.OtherComment;
        }
    }
}
