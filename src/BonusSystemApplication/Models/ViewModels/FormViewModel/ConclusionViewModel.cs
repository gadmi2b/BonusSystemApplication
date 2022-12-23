namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    public class ConclusionViewModel
    {
        public string OverallKpi { get; set; } = string.Empty;
        public bool IsProposalForBonusPayment { get; set; } = false;
        public string ManagerComment { get; set; } = string.Empty;
        public string EmployeeComment { get; set; } = string.Empty;
        public string OtherComment { get; set; } = string.Empty;

        public ConclusionViewModel(Conclusion source)
        {
            OverallKpi = source.OverallKpi == null ? string.Empty : source.OverallKpi;
            IsProposalForBonusPayment = source.IsProposalForBonusPayment;
            ManagerComment = source.ManagerComment == null ? string.Empty : source.ManagerComment;
            EmployeeComment = source.EmployeeComment == null ? string.Empty : source.EmployeeComment;
            OtherComment = source.OtherComment == null ? string.Empty : source.OtherComment;
        }
    }
}
