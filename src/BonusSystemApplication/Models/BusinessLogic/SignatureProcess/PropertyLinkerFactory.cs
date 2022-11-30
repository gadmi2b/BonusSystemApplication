namespace BonusSystemApplication.Models.BusinessLogic.SignatureProcess
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
                IdPairsIsSignedIsRejected = new Dictionary<string, string?>()
                {
                    { nameof(Form.Signatures.ForObjectives.IsSignedByEmployee), nameof(Form.Signatures.ForObjectives.IsRejectedByEmployee)},
                    { nameof(Form.Signatures.ForObjectives.IsSignedByManager), null},
                    { nameof(Form.Signatures.ForObjectives.IsSignedByApprover), null},
                },
                IdPairsIsSignedSignature = new Dictionary<string, string?>()
                {
                    { nameof(Form.Signatures.ForObjectives.IsSignedByEmployee), nameof(Form.Signatures.ForObjectives.EmployeeSignature)},
                    { nameof(Form.Signatures.ForObjectives.IsSignedByManager), nameof(Form.Signatures.ForObjectives.ManagerSignature)},
                    { nameof(Form.Signatures.ForObjectives.IsSignedByApprover), nameof(Form.Signatures.ForObjectives.ApproverSignature)},
                }
            };
        }

        private static IPropertyLinker GetResultsSignaturePropertyLinker()
        {
            return new PropertyLinker()
            {
                PropertyType = PropertyTypes.Results,
                IdPairsIsSignedIsRejected = new Dictionary<string, string?>()
                {
                    { nameof(Form.Signatures.ForResults.IsSignedByEmployee), nameof(Form.Signatures.ForResults.IsRejectedByEmployee)},
                    { nameof(Form.Signatures.ForResults.IsSignedByManager), null},
                    { nameof(Form.Signatures.ForResults.IsSignedByApprover), null},
                },
                IdPairsIsSignedSignature = new Dictionary<string, string?>()
                {
                    { nameof(Form.Signatures.ForResults.IsSignedByEmployee), nameof(Form.Signatures.ForResults.EmployeeSignature)},
                    { nameof(Form.Signatures.ForResults.IsSignedByManager), nameof(Form.Signatures.ForResults.ManagerSignature)},
                    { nameof(Form.Signatures.ForResults.IsSignedByApprover), nameof(Form.Signatures.ForResults.ApproverSignature)},
                }
            };
        }

    }
}
