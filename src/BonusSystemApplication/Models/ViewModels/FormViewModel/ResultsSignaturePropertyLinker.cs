namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    public class ResultsSignaturePropertyLinker : IPropertyLinker
    {
        // link between all IsSigned and IsRejected properties
        public Dictionary<string, string?> IsSignedIsRejectedPairs { get; } = new Dictionary<string, string?>()
            {
                { nameof(Form.IsResultsSignedByEmployee), nameof(Form.IsResultsRejectedByEmployee)},
                { nameof(Form.IsResultsSignedByManager), null},
                { nameof(Form.IsResultsSignedByApprover), null},
            };
        // link between all IsSigned and Signature properties
        public Dictionary<string, string?> IsSignedSignaturePairs { get; } = new Dictionary<string, string?>()
            {
                { nameof(Form.IsResultsSignedByEmployee), nameof(Form.ResultsEmployeeSignature)},
                { nameof(Form.IsResultsSignedByManager), nameof(Form.ResultsManagerSignature)},
                { nameof(Form.IsResultsSignedByApprover), nameof(Form.ResultsApproverSignature)},
            };
    }
}
