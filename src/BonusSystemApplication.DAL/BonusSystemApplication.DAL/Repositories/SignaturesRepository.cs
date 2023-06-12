using BonusSystemApplication.DAL.EF;
using BonusSystemApplication.DAL.Entities;
using BonusSystemApplication.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BonusSystemApplication.DAL.Repositories
{
    public class SignaturesRepository : ISignaturesRepository
    {
        private DataContext _context;
        public SignaturesRepository(DataContext ctx) => _context = ctx;

        public Signatures GetSignatures(long formId)
        {
            return _context.Signatures
                .AsNoTracking()
                .Where(s => s.FormId == formId)
                .First();
        }

        public void DropSignatures(long formId)
        {
            Signatures signatures = _context.Signatures
                .Where(s => s.FormId == formId)
                .First();

            ArgumentNullException.ThrowIfNull(signatures, nameof(signatures));

            signatures.ForObjectivesIsSignedByEmployee = false;
            signatures.ForObjectivesEmployeeSignature = string.Empty;
            signatures.ForObjectivesIsRejectedByEmployee = false;
            signatures.ForObjectivesIsSignedByManager = false;
            signatures.ForObjectivesManagerSignature = string.Empty;
            signatures.ForObjectivesIsSignedByApprover = false;
            signatures.ForObjectivesApproverSignature = string.Empty;

            signatures.ForResultsIsSignedByEmployee = false;
            signatures.ForResultsEmployeeSignature = string.Empty;
            signatures.ForResultsIsRejectedByEmployee = false;
            signatures.ForResultsIsSignedByManager = false;
            signatures.ForResultsManagerSignature = string.Empty;
            signatures.ForResultsIsSignedByApprover = false;
            signatures.ForResultsApproverSignature = string.Empty;

            _context.SaveChanges();
        }
        public void DropSignaturesForResults(long formId)
        {
            Signatures signatures = _context.Signatures
                .Where(s => s.FormId == formId)
                .First();

            ArgumentNullException.ThrowIfNull(signatures, nameof(signatures));

            signatures.ForResultsIsSignedByEmployee = false;
            signatures.ForResultsEmployeeSignature = string.Empty;
            signatures.ForResultsIsRejectedByEmployee = false;
            signatures.ForResultsIsSignedByManager = false;
            signatures.ForResultsManagerSignature = string.Empty;
            signatures.ForResultsIsSignedByApprover = false;
            signatures.ForResultsApproverSignature = string.Empty;

            _context.SaveChanges();
        }
    }
}
