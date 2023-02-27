using System.ComponentModel.DataAnnotations.Schema;

namespace BonusSystemApplication.DAL.Entities
{
    public class Signatures
    {
        public long Id { get; set; }

        public string? ForObjectivesEmployeeSignature { get; set; }
        public bool ForObjectivesIsSignedByEmployee { get; set; }
        public bool ForObjectivesIsRejectedByEmployee { get; set; }
        public string? ForObjectivesManagerSignature { get; set; }
        public bool ForObjectivesIsSignedByManager { get; set; }
        public string? ForObjectivesApproverSignature { get; set; }
        public bool ForObjectivesIsSignedByApprover { get; set; }
        [NotMapped]
        public bool IsObjectivesSigned
        {
            get => ForObjectivesIsSignedByEmployee &
                   ForObjectivesIsSignedByManager &
                   ForObjectivesIsSignedByApprover;
        }

        public string? ForResultsEmployeeSignature { get; set; }
        public bool ForResultsIsSignedByEmployee { get; set; }
        public bool ForResultsIsRejectedByEmployee { get; set; }
        public string? ForResultsManagerSignature { get; set; }
        public bool ForResultsIsSignedByManager { get; set; }
        public string? ForResultsApproverSignature { get; set; }
        public bool ForResultsIsSignedByApprover { get; set; }
        [NotMapped]
        public bool IsResultsSigned
        {
            get => ForResultsIsSignedByEmployee &
                   ForResultsIsSignedByManager &
                   ForResultsIsSignedByApprover;
        }

        public long FormId { get; set; }
        public Form Form { get; set; }
    }
}
