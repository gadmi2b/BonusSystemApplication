using BonusSystemApplication.DAL.Entities;

namespace BonusSystemApplication.BLL.DTO.Edit
{
    public class SignaturesDTO
    {
        public long Id { get; set; }

        public string ForObjectivesEmployeeSignature { get; set; } = string.Empty;
        public bool ForObjectivesIsSignedByEmployee { get; set; }
        public bool ForObjectivesIsRejectedByEmployee { get; set; }
        public string ForObjectivesManagerSignature { get; set; } = string.Empty;
        public bool ForObjectivesIsSignedByManager { get; set; }
        public string ForObjectivesApproverSignature { get; set; } = string.Empty;
        public bool ForObjectivesIsSignedByApprover { get; set; }
        public bool IsObjectivesSigned
        {
            get => ForObjectivesIsSignedByEmployee &
                   ForObjectivesIsSignedByManager &
                   ForObjectivesIsSignedByApprover;
        }

        public string ForResultsEmployeeSignature { get; set; } = string.Empty;
        public bool ForResultsIsSignedByEmployee { get; set; }
        public bool ForResultsIsRejectedByEmployee { get; set; }
        public string ForResultsManagerSignature { get; set; } = string.Empty;
        public bool ForResultsIsSignedByManager { get; set; }
        public string ForResultsApproverSignature { get; set; } = string.Empty;
        public bool ForResultsIsSignedByApprover { get; set; }
        public bool IsResultsSigned
        {
            get => ForResultsIsSignedByEmployee &
                   ForResultsIsSignedByManager &
                   ForResultsIsSignedByApprover;
        }

        public SignaturesDTO(Signatures source)
        {
            Id = source.Id;

            ForObjectivesEmployeeSignature = source.ForObjectivesEmployeeSignature == null ? string.Empty : source.ForObjectivesEmployeeSignature;
            ForObjectivesIsSignedByEmployee = source.ForObjectivesIsSignedByEmployee;
            ForObjectivesIsRejectedByEmployee = source.ForObjectivesIsRejectedByEmployee;
            ForObjectivesManagerSignature = source.ForObjectivesManagerSignature == null ? string.Empty : source.ForObjectivesManagerSignature;
            ForObjectivesIsSignedByManager = source.ForObjectivesIsSignedByManager;
            ForObjectivesApproverSignature = source.ForObjectivesApproverSignature == null ? string.Empty : source.ForObjectivesApproverSignature;
            ForObjectivesIsSignedByApprover = source.ForObjectivesIsSignedByApprover;

            ForResultsEmployeeSignature = source.ForResultsEmployeeSignature == null ? string.Empty : source.ForResultsEmployeeSignature;
            ForResultsIsSignedByEmployee = source.ForResultsIsSignedByEmployee;
            ForResultsIsRejectedByEmployee = source.ForResultsIsRejectedByEmployee;
            ForResultsManagerSignature = source.ForResultsManagerSignature == null ? string.Empty : source.ForResultsManagerSignature;
            ForResultsIsSignedByManager = source.ForResultsIsSignedByManager;
            ForResultsApproverSignature = source.ForResultsApproverSignature == null ? string.Empty : source.ForResultsApproverSignature;
            ForResultsIsSignedByApprover = source.ForResultsIsSignedByApprover;
        }
    }
}
