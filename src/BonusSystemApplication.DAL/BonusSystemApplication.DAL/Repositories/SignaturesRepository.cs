using Microsoft.EntityFrameworkCore;
using BonusSystemApplication.DAL.EF;
using BonusSystemApplication.DAL.Entities;
using BonusSystemApplication.DAL.Interfaces;

namespace BonusSystemApplication.DAL.Repositories
{
    public class SignaturesRepository : ISignaturesRepository
    {
        private DataContext context;
        public SignaturesRepository(DataContext ctx) => context = ctx;

        public Signatures GetSignatures(long formId)
        {
            return context.Signatures.TagWith($"Get Signatures for FormId: {formId}")
                .Where(s => s.FormId == formId)
                .First();
        }

        public void DropSignatures(long formId)
        {
            Signatures dbSignatures = context.Signatures.TagWith($"Get Signatures for FormId: {formId}")
                .Where(s => s.FormId == formId)
                .First();

            if (dbSignatures == null)
                throw new Exception($"{nameof(SignaturesRepository)}. Method: {nameof(DropSignatures)}. " +
                                    $"Can't find {nameof(Signatures)} object for formId = {formId}");
                
            dbSignatures.ForObjectivesIsSignedByEmployee = false;
            dbSignatures.ForObjectivesEmployeeSignature = string.Empty;
            dbSignatures.ForObjectivesIsRejectedByEmployee = false;
            dbSignatures.ForObjectivesIsSignedByManager = false;
            dbSignatures.ForObjectivesManagerSignature = string.Empty;
            dbSignatures.ForObjectivesIsSignedByApprover = false;
            dbSignatures.ForObjectivesApproverSignature = string.Empty;

            dbSignatures.ForResultsIsSignedByEmployee = false;
            dbSignatures.ForResultsEmployeeSignature = string.Empty;
            dbSignatures.ForResultsIsRejectedByEmployee = false;
            dbSignatures.ForResultsIsSignedByManager = false;
            dbSignatures.ForResultsManagerSignature = string.Empty;
            dbSignatures.ForResultsIsSignedByApprover = false;
            dbSignatures.ForResultsApproverSignature = string.Empty;

            context.SaveChanges();
        }
        public void DropSignaturesForResults(long formId)
        {
            Signatures dbSignatures = context.Signatures.TagWith($"Get Signatures for FormId: {formId}")
                .Where(s => s.FormId == formId)
                .First();

            if (dbSignatures == null)
                throw new Exception($"{nameof(SignaturesRepository)}. Method: {nameof(DropSignaturesForResults)}. " +
                                    $"Can't find {nameof(Signatures)} object for formId = {formId}");

            dbSignatures.ForResultsIsSignedByEmployee = false;
            dbSignatures.ForResultsEmployeeSignature = string.Empty;
            dbSignatures.ForResultsIsRejectedByEmployee = false;
            dbSignatures.ForResultsIsSignedByManager = false;
            dbSignatures.ForResultsManagerSignature = string.Empty;
            dbSignatures.ForResultsIsSignedByApprover = false;
            dbSignatures.ForResultsApproverSignature = string.Empty;

            context.SaveChanges();
        }
    }
}
