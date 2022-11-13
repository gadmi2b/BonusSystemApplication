namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    public class ObjectivesSignaturePropertyLinker : IPropertyLinker
    {
        // link between all IsSigned and IsRejected properties
        public Dictionary<string, string?> IsSignedIsRejectedPairs { get; } = new Dictionary<string, string?>()
            {
                { nameof(Form.IsObjectivesSignedByEmployee), nameof(Form.IsObjectivesRejectedByEmployee)},
                { nameof(Form.IsObjectivesSignedByManager), null},
                { nameof(Form.IsObjectivesSignedByApprover), null},
            };
        // link between all IsSigned and Signature properties
        public Dictionary<string, string?> IsSignedSignaturePairs { get; } = new Dictionary<string, string?>()
            {
                { nameof(Form.IsObjectivesSignedByEmployee), nameof(Form.ObjectivesEmployeeSignature)},
                { nameof(Form.IsObjectivesSignedByManager), nameof(Form.ObjectivesManagerSignature)},
                { nameof(Form.IsObjectivesSignedByApprover), nameof(Form.ObjectivesApproverSignature)},
            };
    }
}
