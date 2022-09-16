namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    /// <summary>
    /// Stage #4 of form defenition: results were freezed and signatures collection is in progress 
    /// </summary>
    public class ResultsSignature
    {
        public string ResultsEmployeeSignature { get; set; } = string.Empty;
        public bool IsResultsSignedByEmployee { get; set; } = false;
        public bool IsResultsRejectedByEmployee { get; set; } = false;
        public string ResultsManagerSignature { get; set; } = string.Empty;
        public bool IsResultsSignedByManager { get; set; } = false;
        public string ResultsApproverSignature { get; set; } = string.Empty;
        public bool IsResultsSignedByApprover { get; set; } = false;

        public bool IsResultsSigned
        {
            get
            {
                return IsResultsSignedByEmployee &&
                       IsResultsSignedByManager &&
                       IsResultsSignedByApprover;
            }
        }
    }
}
