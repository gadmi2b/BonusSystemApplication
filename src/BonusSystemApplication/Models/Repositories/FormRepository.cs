using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient.Server;
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

        public IQueryable<Form> GetFormsWithGlobalAccess(IEnumerable<FormGlobalAccess> formGlobalAccesses)
        {
            IQueryable<Form> formsQueryInitial = context.Forms.AsQueryable()
                .Include(f => f.Employee)
                .Include(f => f.Workproject)
                .Include(f => f.FormLocalAccess)
                .AsNoTracking();

            IQueryable<Form> formsQuery = null;

            foreach (var formGA in formGlobalAccesses)
            {
                IQueryable <Form> query = formsQueryInitial.Where(ExpressionBuilder.GetExpressionForGlobalAccess(formGA));

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
        public IQueryable<Form> GetFormsWithLocalAccess(long userId)
        {
            IQueryable<Form> forms = context.Forms.AsQueryable()
                .Include(f => f.Employee)
                .Include(f => f.Workproject)
                .Include(f => f.FormLocalAccess)
                .Where(ExpressionBuilder.GetExpressionForLocalAccess(userId))
                .AsNoTracking();
            return forms;
        }
        public IQueryable<Form> GetFormsWithParticipation(long userId)
        {
            IQueryable<Form> forms = context.Forms.AsQueryable()
                .Include(f => f.Employee)
                .Include(f => f.Workproject)
                .Include(f => f.FormLocalAccess)
                .Where(ExpressionBuilder.GetExpressionForParticipation(userId))
                .AsNoTracking();
            return forms;
        }
    }
}
