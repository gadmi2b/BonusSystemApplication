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

        public async Task<Signatures> GetSignaturesAsync(long formId)
        {
            return await _context.Signatures.AsNoTracking()
                .Where(s => s.FormId == formId)
                .FirstAsync();
        }

        public async Task DropSignaturesAsync(long formId)
        {
            Signatures? signatures = await _context.Signatures
                .Where(s => s.FormId == formId)
                .FirstAsync();

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

            await _context.SaveChangesAsync();
        }
        public async Task DropSignaturesForResultsAsync(long formId)
        {
            Signatures? signatures = await _context.Signatures
                .Where(s => s.FormId == formId)
                .FirstAsync();

            ArgumentNullException.ThrowIfNull(signatures, nameof(signatures));

            signatures.ForResultsIsSignedByEmployee = false;
            signatures.ForResultsEmployeeSignature = string.Empty;
            signatures.ForResultsIsRejectedByEmployee = false;
            signatures.ForResultsIsSignedByManager = false;
            signatures.ForResultsManagerSignature = string.Empty;
            signatures.ForResultsIsSignedByApprover = false;
            signatures.ForResultsApproverSignature = string.Empty;

            await _context.SaveChangesAsync();
        }
    }
}
