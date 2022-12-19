using Microsoft.EntityFrameworkCore;

namespace BonusSystemApplication.Models.Repositories
{
    public class DefinitionRepository : IDefinitionRepository
    {
        private DataContext context;
        public DefinitionRepository(DataContext ctx) => context = ctx;

        public List<long> GetParticipationFormIds(long userId)
        {
            return context.Definitions.TagWith("Get form ids with Participation access")
                .Where(d => d.EmployeeId == userId || d.ManagerId == userId || d.ApproverId == userId)
                .Select(d => d.Id)
                .ToList();
        }
        public List<long> GetGlobalAccessFormIds(IEnumerable<GlobalAccess> globalAccesses)
        {
            IQueryable<Definition> queryInitial = context.Definitions.AsQueryable().TagWith("Get form ids with Global access")
                .Include(d => d.Employee)
                    .ThenInclude(e => e.Department)
                .Include(d => d.Employee)
                    .ThenInclude(e => e.Team)
                .Include(d => d.Workproject)
                .AsNoTracking();

            IQueryable<Definition> queryFiltered = null;

            foreach (var gAccess in globalAccesses)
            {
                IQueryable<Definition> query = queryInitial.Where(ExpressionBuilder.GetExpressionForGlobalAccess(gAccess));

                if (query == null)
                {
                    continue;
                }

                if (queryFiltered == null)
                {
                    queryFiltered = query;
                }
                else
                {
                    queryFiltered = queryFiltered.Union(query);
                }
            }

            if(queryFiltered == null)
            {
                return new List<long>();
            }

            return queryFiltered
                .Select(d => d.Id)
                .ToList();
        }
    }
}
