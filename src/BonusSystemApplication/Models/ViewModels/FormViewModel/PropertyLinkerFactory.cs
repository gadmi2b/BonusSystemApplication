namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    public static class PropertyLinkerFactory
    {
        public static IPropertyLinker CreatePropertyLinker(PropertyTypes propertyType)
        {
            switch (propertyType)
            {
                case PropertyTypes.Objectives:
                    return GetObjectivesSignaturePropertyLinker();

                case PropertyTypes.Results:
                    return GetResultsSignaturePropertyLinker();

                default: throw new ArgumentException("Unknown type of property");
            }
        }

        private static IPropertyLinker GetObjectivesSignaturePropertyLinker()
        {
            return new PropertyLinker()
            {
                PropertyType = PropertyTypes.Objectives,
                IsSignedIsRejectedPairs = new Dictionary<string, string?>()
                {
                    { nameof(Form.IsObjectivesSignedByEmployee), nameof(Form.IsObjectivesRejectedByEmployee)},
                    { nameof(Form.IsObjectivesSignedByManager), null},
                    { nameof(Form.IsObjectivesSignedByApprover), null},
                },
                IsSignedSignaturePairs = new Dictionary<string, string?>()
                {
                    { nameof(Form.IsObjectivesSignedByEmployee), nameof(Form.ObjectivesEmployeeSignature)},
                    { nameof(Form.IsObjectivesSignedByManager), nameof(Form.ObjectivesManagerSignature)},
                    { nameof(Form.IsObjectivesSignedByApprover), nameof(Form.ObjectivesApproverSignature)},
                }
            };
        }

        private static IPropertyLinker GetResultsSignaturePropertyLinker()
        {
            return new PropertyLinker()
            {
                PropertyType = PropertyTypes.Results,
                IsSignedIsRejectedPairs = new Dictionary<string, string?>()
                {
                    { nameof(Form.IsResultsSignedByEmployee), nameof(Form.IsResultsRejectedByEmployee)},
                    { nameof(Form.IsResultsSignedByManager), null},
                    { nameof(Form.IsResultsSignedByApprover), null},
                },
                IsSignedSignaturePairs = new Dictionary<string, string?>()
                {
                    { nameof(Form.IsResultsSignedByEmployee), nameof(Form.ResultsEmployeeSignature)},
                    { nameof(Form.IsResultsSignedByManager), nameof(Form.ResultsManagerSignature)},
                    { nameof(Form.IsResultsSignedByApprover), nameof(Form.ResultsApproverSignature)},
                }
            };
        }

    }
}
