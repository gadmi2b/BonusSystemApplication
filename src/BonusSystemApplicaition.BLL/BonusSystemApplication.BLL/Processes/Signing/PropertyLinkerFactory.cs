using BonusSystemApplication.BLL.Interfaces;
using BonusSystemApplication.DAL.Entities;

namespace BonusSystemApplication.BLL.Processes.Signing
{
    public class PropertyLinkerFactory
    {
        public IPropertyLinker CreatePropertyLinker(PropertyType propertyType)
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


        private IPropertyLinker GetObjectivesSignaturePropertyLinker()
        {
            return new PropertyLinker()
            {
                PropertyType = PropertyType.ForObjectives,
                IdPairsIsSignedIsRejected = new Dictionary<string, string?>()
                {
                    { nameof(Signatures.ForObjectivesIsSignedByEmployee), nameof(Signatures.ForObjectivesIsRejectedByEmployee)},
                    { nameof(Signatures.ForObjectivesIsSignedByManager), null},
                    { nameof(Signatures.ForObjectivesIsSignedByApprover), null},
                },
                IdPairsIsSignedSignature = new Dictionary<string, string?>()
                {
                    { nameof(Signatures.ForObjectivesIsSignedByEmployee), nameof(Signatures.ForObjectivesEmployeeSignature)},
                    { nameof(Signatures.ForObjectivesIsSignedByManager),  nameof(Signatures.ForObjectivesManagerSignature)},
                    { nameof(Signatures.ForObjectivesIsSignedByApprover), nameof(Signatures.ForObjectivesApproverSignature)},
                }
            };
        }
        private IPropertyLinker GetResultsSignaturePropertyLinker()
        {
            return new PropertyLinker()
            {
                PropertyType = PropertyType.ForResults,
                IdPairsIsSignedIsRejected = new Dictionary<string, string?>()
                {
                    { nameof(Signatures.ForResultsIsSignedByEmployee), nameof(Signatures.ForResultsIsRejectedByEmployee)},
                    { nameof(Signatures.ForResultsIsSignedByManager), null},
                    { nameof(Signatures.ForResultsIsSignedByApprover), null},
                },
                IdPairsIsSignedSignature = new Dictionary<string, string?>()
                {
                    { nameof(Signatures.ForResultsIsSignedByEmployee), nameof(Signatures.ForResultsEmployeeSignature)},
                    { nameof(Signatures.ForResultsIsSignedByManager),  nameof(Signatures.ForResultsManagerSignature)},
                    { nameof(Signatures.ForResultsIsSignedByApprover), nameof(Signatures.ForResultsApproverSignature)},
                }
            };
        }
    }
}
