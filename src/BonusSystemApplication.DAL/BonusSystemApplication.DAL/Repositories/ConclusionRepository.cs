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
            return context.Conclusions.TagWith($"Get Conclusion for FormId: {formId}")
                    .Where(c => c.FormId == formId)
                    .Select(c => new Conclusion
                    {
                        OverallKpi = c.OverallKpi,
                        IsProposalForBonusPayment = c.IsProposalForBonusPayment,
                        ManagerComment = c.ManagerComment,
                        EmployeeComment = c.EmployeeComment,
                        OtherComment = c.OtherComment,
                    })
                    .First();
        }
    }
}
