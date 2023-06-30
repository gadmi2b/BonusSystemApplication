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

        public async Task<List<ObjectiveResult>> GetObjectivesResultsAsync(long formId)
        {
            return await _context.ObjectivesResults.AsNoTracking()
                    .Where(or => or.FormId == formId)
                    .OrderBy(or => or.Row)
                    .ToListAsync();
        }
    }
}
