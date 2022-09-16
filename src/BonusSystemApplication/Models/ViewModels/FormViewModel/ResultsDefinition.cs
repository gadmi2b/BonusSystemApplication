namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    /// <summary>
    /// Stage #3 of form defenition: form results, proposal for bonus payment and comments 
    /// </summary>
    public class ResultsDefinition
    {
        public bool IsResultsFreezed { get; set; } = false;
        public string OverallKpi { get; set; } = string.Empty;
        public bool IsProposalForBonusPayment { get; set; } = false;
        public string ManagerComment { get; set; } = string.Empty;
        public string EmployeeComment { get; set; } = string.Empty;
        public string OtherComment { get; set; } = string.Empty;
        public IList<ObjectiveResult> ObjectivesResults { get; set; } = new List<ObjectiveResult>();
    }
}
