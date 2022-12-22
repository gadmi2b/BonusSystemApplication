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
                IdPairsIsSignedIsRejected = new Dictionary<string, string?>()
                {
                    { ForResults.ToStringIsSignedByEmployee(), ForResults.ToStringIsRejectedByEmployee()},
                    { ForResults.ToStringIsSignedByManager(), null},
                    { ForResults.ToStringIsSignedByApprover(), null},
                },
                IdPairsIsSignedSignature = new Dictionary<string, string?>()
                {
                    { ForResults.ToStringIsSignedByEmployee(), ForResults.ToStringEmployeeSignature()},
                    { ForResults.ToStringIsSignedByManager(), ForResults.ToStringManagerSignature()},
                    { ForResults.ToStringIsSignedByApprover(), ForResults.ToStringApproverSignature()},
                }
            };
        }
    }
}
