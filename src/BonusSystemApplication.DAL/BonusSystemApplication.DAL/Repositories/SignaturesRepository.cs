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
    }
}
