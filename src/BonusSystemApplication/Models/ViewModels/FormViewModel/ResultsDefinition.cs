namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    /// <summary>
    /// Stage #3 of form definition: form results, proposal for bonus payment and comments 
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

        public ResultsDefinition(Form form)
        {
            if(form.IsObjectivesFreezed &&
               form.IsObjectivesSignedByEmployee &&
               form.IsObjectivesSignedByManager &&
               form.IsObjectivesSignedByApprover)
            {
                IsResultsFreezed = form.IsResultsFreezed;
                OverallKpi = form.OverallKpi;
                IsProposalForBonusPayment = form.IsProposalForBonusPayment;
                ManagerComment = form.ManagerComment;
                EmployeeComment = form.EmployeeComment;
                OtherComment = form.OtherComment;
                ObjectivesResults = form.ObjectivesResults
                    .Select(x => new ObjectiveResult
                    {
                        KeyCheck = x.KeyCheck,
                        Achieved = x.Achieved,
                        Kpi = x.Kpi,
                    })
                    .ToList();
            }
            else
            {
                // just keep default values
                for(int i = 0; i < 10; i++)
                {
                    ObjectiveResult result = new ObjectiveResult
                    {
                        KeyCheck = string.Empty,
                        Achieved = string.Empty,
                        Kpi = null,
                    };
                    ObjectivesResults.Add(result);
                }
            }
        }
    }
}