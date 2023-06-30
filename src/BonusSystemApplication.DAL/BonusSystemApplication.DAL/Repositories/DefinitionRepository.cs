using BonusSystemApplication.DAL.Interfaces;
using BonusSystemApplication.DAL.Entities;
using BonusSystemApplication.DAL.EF;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BonusSystemApplication.DAL.Repositories
{
    public class DefinitionRepository : IDefinitionRepository
    {
        private DataContext _context;
        public DefinitionRepository(DataContext ctx) => _context = ctx;

        public async Task<Definition> GetDefinitionAsync(long formId)
        {
            return await _context.Definitions.AsNoTracking()
                    .Where(d => d.FormId == formId)
                    .FirstAsync();
        }
        public async Task<List<long>> GetFormIdsWhereParticipationAsync(long userId)
        {
            return await _context.Definitions.AsNoTracking()
                    .Where(d => d.EmployeeId == userId || d.ManagerId == userId || d.ApproverId == userId)
                    .Select(d => d.FormId)
                    .ToListAsync();
        }
        public async Task<List<long>> GetFormIdsWhereGlobalAccessAsync(IEnumerable<GlobalAccess> globalAccesses)
        {
            IQueryable<Definition> queryInitial = _context.Definitions.AsQueryable()
                .Include(d => d.Employee)
                    .ThenInclude(e => e.Department)
                .Include(d => d.Employee)
                    .ThenInclude(e => e.Team)
                .Include(d => d.Workproject)
                .AsNoTracking();

            IQueryable<Definition> queryFiltered = null!;

            foreach (var globalAccess in globalAccesses)
            {
                IQueryable<Definition> query = queryInitial.Where(GetGlobalAccessExpression(globalAccess));

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

            if (queryFiltered == null)
            {
                return new List<long>();
            }

            return await queryFiltered
                .Select(d => d.FormId)
                .ToListAsync();
        }

        /// <summary>
        /// Checks if form with another formId exists in database
        /// with same properties
        /// </summary>
        /// <param name="formId"></param>
        /// <param name="employeeId"></param>
        /// <param name="workprojectId"></param>
        /// <param name="year"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public async Task<bool> IsExistWithSamePropertyCombinationAsync(long formId,
                                                                        long employeeId,
                                                                        long workprojectId,
                                                                        int year,
                                                                        Periods period)
        {
            long originalFormId = await _context.Definitions
                        .Where(d => d.EmployeeId == employeeId &&
                                    d.WorkprojectId == workprojectId &&
                                    d.Period == period &&
                                    d.Year == year)
                        .Select(d => d.FormId)
                        .FirstOrDefaultAsync();

            if (originalFormId == 0 ||
                originalFormId == formId)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        /// <summary>
        /// Creates ea expression for quering formIds depending on GlobalAccess object
        /// </summary>
        /// <param name="gAccess">A Global Access object</param>
        /// <returns>An expression for FormIds quering</returns>
        private Expression<Func<Definition, bool>> GetGlobalAccessExpression(GlobalAccess gAccess)
        {
            /* Logic description
            There are 3 big areas of global access working like filters if presented (from biggest to smallest):
             - Department (like Engineering or IT)
             - Team (like Design or Stress)
             - Workproject (like 145)

            If userId is presented in the global accesses without any areas - it will get access to all forms
            If only DepartmentId is presented - gets an access to forms from this Department
            If TeamId is presented also - gets an access to this Team's forms inside this Department
            If WorkprojectsId is indicated - gets an access to forms of this Workproject inside Team inside Department
            */

            Expression<Func<Definition, bool>> expr = (d) => false;

            if (gAccess.DepartmentId == null)
            {
                expr = (d) => true;
            }
            else if (gAccess.TeamId == null)
            {
                expr = (d) => d.Employee.DepartmentId == gAccess.DepartmentId;
            }
            else if (gAccess.WorkprojectId == null)
            {
                expr = (d) => d.Employee.DepartmentId == gAccess.DepartmentId &&
                              d.Employee.TeamId == gAccess.TeamId;
            }
            else
            {
                expr = (d) => d.Employee.DepartmentId == gAccess.DepartmentId &&
                              d.Employee.TeamId == gAccess.TeamId &&
                              d.WorkprojectId == gAccess.WorkprojectId;
            }

            return expr;
        }
    }
}
