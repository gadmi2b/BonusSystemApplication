namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    /// <summary>
    /// Stage #3 of form definition: form results, proposal for bonus payment and comments 
    /// </summary>
    public class Conclusion
    {
        public string OverallKpi { get; set; } = string.Empty;
        public bool IsProposalForBonusPayment { get; set; } = false;
        public string ManagerComment { get; set; } = string.Empty;
        public string EmployeeComment { get; set; } = string.Empty;
        public string OtherComment { get; set; } = string.Empty;

        public Conclusion() { }
        public Conclusion(Form form)
        {
            OverallKpi = form.OverallKpi;
            IsProposalForBonusPayment = form.IsProposalForBonusPayment;
            ManagerComment = form.ManagerComment;
            EmployeeComment = form.EmployeeComment;
            OtherComment = form.OtherComment;
        }
    }
}