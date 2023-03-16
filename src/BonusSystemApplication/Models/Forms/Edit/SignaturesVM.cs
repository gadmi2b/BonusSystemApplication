namespace BonusSystemApplication.Models.Forms.Edit
{
    public class SignaturesVM
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

        public SignaturesVM() { }
    }
}
