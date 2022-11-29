namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    /// <summary>
    /// Stage #4 of form definition: results were freezed and signatures collection is in progress 
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

        public ResultsSignature() { }
        public ResultsSignature(Form form)
        {
            IsResultsSignedByEmployee = form.IsResultsSignedByEmployee;
            ResultsEmployeeSignature = form.ResultsEmployeeSignature == null ? string.Empty : form.ResultsEmployeeSignature;
            IsResultsRejectedByEmployee = form.IsResultsRejectedByEmployee;
            IsResultsSignedByManager = form.IsResultsSignedByManager;
            ResultsManagerSignature = form.ResultsManagerSignature == null ? string.Empty : form.ResultsManagerSignature;
            IsResultsSignedByApprover = form.IsResultsSignedByApprover;
            ResultsApproverSignature = form.ResultsApproverSignature == null ? string.Empty : form.ResultsApproverSignature;
        }
    }
}
