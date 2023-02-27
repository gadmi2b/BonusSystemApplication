using Microsoft.EntityFrameworkCore;
using BonusSystemApplication.DAL.EF;
using BonusSystemApplication.DAL.Entities;
using BonusSystemApplication.DAL.Interfaces;

namespace BonusSystemApplication.DAL.Repositories
{
    public class ConclusionRepository : IConclusionRepository
    {
        private DataContext context;
        public ConclusionRepository(DataContext ctx) => context = ctx;

        public Conclusion GetConclusion(long formId)
        {
            return context.Forms.TagWith($"Get Conclusion for FormId: {formId}")
                    .Where(f => f.Id == formId)
                    .Select(f => new Conclusion
                    {
                        OverallKpi = f.Conclusion.OverallKpi,
                        IsProposalForBonusPayment = f.Conclusion.IsProposalForBonusPayment,
                        ManagerComment = f.Conclusion.ManagerComment,
                        EmployeeComment = f.Conclusion.EmployeeComment,
                        OtherComment = f.Conclusion.OtherComment,
                    })
                    .First();
        }
    }
}
