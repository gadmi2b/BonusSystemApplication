using BonusSystemApplication.DAL.EF;
using BonusSystemApplication.DAL.Entities;
using BonusSystemApplication.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BonusSystemApplication.DAL.Repositories
{
    public class ObjectiveResultRepository : IObjectiveResultRepository
    {
        private DataContext _context;
        public ObjectiveResultRepository(DataContext ctx) => _context = ctx;

        public List<ObjectiveResult> GetObjectivesResults(long formId)
        {
            return _context.ObjectivesResults
                    .AsNoTracking()
                    .Where(or => or.FormId == formId)
                    .OrderBy(or => or.Row)
                    .ToList();
        }
    }
}
