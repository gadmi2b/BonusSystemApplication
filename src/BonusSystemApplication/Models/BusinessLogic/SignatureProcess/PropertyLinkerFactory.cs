namespace BonusSystemApplication.Models.BusinessLogic.SignatureProcess
{
    public static class PropertyLinkerFactory
    {
        public static IPropertyLinker CreatePropertyLinker(PropertyType propertyType)
        {
            switch (propertyType)
            {
                case PropertyType.ForObjectives:
                    return GetObjectivesSignaturePropertyLinker();

                case PropertyType.ForResults:
                    return GetResultsSignaturePropertyLinker();

                default: throw new ArgumentException("Unknown type of property");
            }
        }

        private static IPropertyLinker GetObjectivesSignaturePropertyLinker()
        {
            return new PropertyLinker()
            {
                PropertyType = PropertyType.ForObjectives,
                PropertyTypeName = nameof(ForObjectives),
                IdPairsIsSignedIsRejected = new Dictionary<string, string?>()
                {
                    { ForObjectives.ToStringIsSignedByEmployee(), ForObjectives.ToStringIsRejectedByEmployee()},
                    { ForObjectives.ToStringIsSignedByManager(), null},
                    { ForObjectives.ToStringIsSignedByApprover(), null},
                },
                IdPairsIsSignedSignature = new Dictionary<string, string?>()
                {
                    { ForObjectives.ToStringIsSignedByEmployee(), ForObjectives.ToStringEmployeeSignature()},
                    { ForObjectives.ToStringIsSignedByManager(), ForObjectives.ToStringManagerSignature()},
                    { ForObjectives.ToStringIsSignedByApprover(), ForObjectives.ToStringApproverSignature()},
                }
            };
        }

        private static IPropertyLinker GetResultsSignaturePropertyLinker()
        {
            return new PropertyLinker()
            {
                PropertyType = PropertyType.ForResults,
                PropertyTypeName = nameof(ForResults),
                IdPairsIsSignedIsRejected = new Dictionary<string, string?>()
                {
                    { nameof(ForResults.IsSignedByEmployee), nameof(ForResults.IsRejectedByEmployee)},
                    { nameof(ForResults.IsSignedByManager), null},
                    { nameof(ForResults.IsSignedByApprover), null},
                },
                IdPairsIsSignedSignature = new Dictionary<string, string?>()
                {
                    { nameof(ForResults.IsSignedByEmployee), nameof(ForResults.EmployeeSignature)},
                    { nameof(ForResults.IsSignedByManager), nameof(ForResults.ManagerSignature)},
                    { nameof(ForResults.IsSignedByApprover), nameof(ForResults.ApproverSignature)},
                }
            };
        }
    }
}
