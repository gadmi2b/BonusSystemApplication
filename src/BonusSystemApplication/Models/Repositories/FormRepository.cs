using Microsoft.EntityFrameworkCore;

namespace BonusSystemApplication.Models.Repositories
{
    public class FormRepository : IFormRepository
    {
        private DataContext context;
        public FormRepository(DataContext ctx) => context = ctx;

        public IEnumerable<Form> GetForm(long id)
        {
            return context.Forms
                .Include(f => f.Employee)
                    .ThenInclude(e => e.Team)
                .Include(f => f.Employee)
                    .ThenInclude(e => e.Position)
                .Include(f => f.Manager)
                .Include(f => f.Approver)
                .Include(f => f.Workproject)
                .Include(f => f.ObjectivesResults)
                .Where(f => f.Id == id)
                .ToList();
        }

        public IEnumerable<Form> GetForms()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Form> GetFormsWithGlobalAccess(IEnumerable<FormGlobalAccess> formGlobalAccesses)
        {
            IQueryable<Form> formsQueryInitial = context.Forms.AsQueryable()
                .Include(f => f.Employee);

            IQueryable<Form> formsQuery = null;



            foreach (var formGA in formGlobalAccesses)
            {
                IQueryable<Form> query;

                if (formGA.DepartmentId == null) // all forms available for user
                {
                    formsQuery = formsQueryInitial;
                    break;
                }
                else if (formGA.TeamId == null) // forms with same: department
                {
                    query = formsQueryInitial.Where(f => f.Employee.DepartmentId == formGA.DepartmentId);
                }
                else if (formGA.WorkprojectId == null) // forms with same: department, team
                {
                    query = formsQueryInitial.Where(f => f.Employee.DepartmentId == formGA.DepartmentId &&
                                                              f.Employee.TeamId == formGA.TeamId);
                }
                else // forms with: same department, team, workproject
                {
                    query = formsQueryInitial.Where(f => f.Employee.DepartmentId == formGA.DepartmentId &&
                                                              f.Employee.TeamId == formGA.TeamId &&
                                                              f.WorkprojectId == formGA.WorkprojectId);
                }

                if (formsQuery == null)
                {
                    formsQuery = query;
                }
                else
                {
                    formsQuery = formsQuery.Union(query);
                }
            }

            // TODO: add null check here or in controller

            return formsQuery;
        }

        public void CreateForm(Form form)
        {
            throw new NotImplementedException();
        }

        public void UpdateForm(Form form)
        {
            throw new NotImplementedException();
        }

        public void DeleteForm(long id)
        {
            throw new NotImplementedException();
        }
    }
}
