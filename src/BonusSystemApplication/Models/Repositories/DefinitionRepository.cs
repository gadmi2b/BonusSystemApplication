using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BonusSystemApplication.Models.Repositories
{
    public class DefinitionRepository : IDefinitionRepository
    {
        private DataContext context;
        public DefinitionRepository(DataContext ctx) => context = ctx;

        public List<long> GetParticipationFormIds(long userId)
        {
            return context.Definitions.TagWith("Form Ids with Participation access requesting")
                .Where(d => d.EmployeeId == userId || d.ManagerId == userId || d.ApproverId == userId)
                .Select(d => d.Id)
                .ToList();
        }
        public List<long> GetGlobalAccessFormIds(IEnumerable<GlobalAccess> globalAccesses)
        {
            IQueryable<Definition> queryInitial = context.Definitions.AsQueryable().TagWith("Form Ids with Global access requesting")
                .Include(d => d.Employee)
                    .ThenInclude(e => e.Department)
                .Include(d => d.Employee)
                    .ThenInclude(e => e.Team)
                .Include(d => d.Workproject)
                .AsNoTracking();

            IQueryable<Definition> queryFiltered = null;

            foreach (var gAccess in globalAccesses)
            {
                IQueryable<Definition> query = queryInitial.Where(GetExpressionForGlobalAccess(gAccess));

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

        /// <summary>
        /// There are 3 big areas of global access working like filters if presented (from biggest to smallest):
        /// Department (like engineering at all), Team (like Design or Stress) or just Workproject
        /// If userId is presented in the global accesses without any areas - it will get access to all forms
        /// If only DepartmentId is presented - gets an access to forms from this Department
        /// If TeamId is presented also - gets an access to this Team's forms inside this Department
        /// If WorkprojectsId is indicated - gets an access to forms of this WP inside Team inside this Department
        /// </summary>
        /// <param name="gAccess">A Global Access object</param>
        /// <returns>An expression for FormIds quering</returns>
        private Expression<Func<Definition, bool>> GetExpressionForGlobalAccess(GlobalAccess gAccess)
        {
            Expression<Func<Definition, bool>> expr = (Definition d) => false;

            if (gAccess.DepartmentId == null)
            {
                expr = (Definition d) => true;
            }
            else if (gAccess.TeamId == null)
            {
                expr = (Definition d) => d.Employee.DepartmentId == gAccess.DepartmentId;
            }
            else if (gAccess.WorkprojectId == null)
            {
                expr = (Definition d) => d.Employee.DepartmentId == gAccess.DepartmentId &&
                                         d.Employee.TeamId == gAccess.TeamId;
            }
            else
            {
                expr = (Definition d) => d.Employee.DepartmentId == gAccess.DepartmentId &&
                                         d.Employee.TeamId == gAccess.TeamId &&
                                         d.WorkprojectId == gAccess.WorkprojectId;
            }

            return expr;
        }
    }
}
