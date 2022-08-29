using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
                IQueryable <Form> query = formsQueryInitial.Where(GenerateGlobalAccessExpression(formGA));

                if(query == null)
                {
                    continue;
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


        public Expression<Func<Form, bool>> GenerateGlobalAccessExpression(FormGlobalAccess formGlobalAccess)
        {
            Expression<Func<Form, bool>> expr;

            if (formGlobalAccess.DepartmentId == null)
            {
                expr = (Form f) => true;
            }
            else if (formGlobalAccess.TeamId == null)
            {
                expr = (Form f) => f.Employee.DepartmentId == formGlobalAccess.DepartmentId;
            }
            else if (formGlobalAccess.WorkprojectId == null)
            {
                expr = (Form f) => f.Employee.DepartmentId == formGlobalAccess.DepartmentId &&
                                   f.Employee.TeamId == formGlobalAccess.TeamId;
            }
            else
            {
                expr = (Form f) => f.Employee.DepartmentId == formGlobalAccess.DepartmentId &&
                                   f.Employee.TeamId == formGlobalAccess.TeamId &&
                                   f.WorkprojectId == formGlobalAccess.WorkprojectId;
            }

            return expr;
        }
    }
}
