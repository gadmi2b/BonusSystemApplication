using Microsoft.EntityFrameworkCore;
using BonusSystemApplication.DAL.EF;
using BonusSystemApplication.DAL.Entities;
using BonusSystemApplication.DAL.Interfaces;

namespace BonusSystemApplication.DAL.Repositories
{
    public class ObjectiveResultRepository : IObjectiveResultRepository
    {
        private DataContext context;
        public ObjectiveResultRepository(DataContext ctx) => context = ctx;

        public IList<ObjectiveResult> GetObjectivesResults(long formId)
        {
            return context.ObjectivesResults.TagWith($"Get ObjectivesResults for FormId: {formId}")
                    .Where(or => or.FormId == formId)
                    .OrderBy(or => or.Row)
                    .ToList();
        }
    }
}
