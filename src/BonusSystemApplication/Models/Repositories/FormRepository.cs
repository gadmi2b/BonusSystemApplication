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

        public Form GetForm(long id)
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
                .First();
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

        public Form GetFormData(long formId)
        {
            return context.Forms
                    .Where(f => f.Id == formId)
                    .Select(f => new Form
                    {
                        // ObjectivesDefinition data block
                        Id = f.Id,
                        IsObjectivesFreezed = f.IsObjectivesFreezed,
                        Employee = new User
                        {
                            Id = f.Employee.Id,
                            FirstNameEng = f.Employee.FirstNameEng,
                            LastNameEng = f.Employee.LastNameEng,
                            Pid = f.Employee.Pid,
                            Team = new Team
                            {
                                Name = f.Employee.Team == null ? string.Empty : f.Employee.Team.Name,
                            },
                            Position = new Position
                            {
                                NameEng = f.Employee.Position == null ? string.Empty : f.Employee.Position.NameEng,
                            },
                        },
                        Manager = new User
                        {
                            Id = f.ManagerId == null ? 0 : (long)f.ManagerId,
                            FirstNameEng = f.Employee.FirstNameEng,
                            LastNameEng = f.Employee.LastNameEng,
                        },
                        Approver = new User
                        {
                            Id = f.ApproverId == null ? 0 : (long)f.ApproverId,
                            FirstNameEng = f.Employee.FirstNameEng,
                            LastNameEng = f.Employee.LastNameEng,
                        },
                        Workproject = new Workproject
                        {
                            Id = f.WorkprojectId == null ? 0 : (long)f.WorkprojectId,
                            Name = f.Workproject == null ? string.Empty : f.Workproject.Name,
                            Description = f.Workproject == null ? string.Empty : f.Workproject.Description,
                        },
                        Period = f.Period,
                        Year = f.Year,
                        IsWpmHox = f.IsWpmHox,
                        ObjectivesResults = f.ObjectivesResults,

                        // ObjectivesSignature data block
                        IsObjectivesSignedByEmployee = f.IsObjectivesSignedByEmployee,
                        ObjectivesEmployeeSignature = f.ObjectivesEmployeeSignature,
                        IsObjectivesRejectedByEmployee = f.IsObjectivesRejectedByEmployee,
                        IsObjectivesSignedByManager = f.IsObjectivesSignedByManager,
                        ObjectivesManagerSignature = f.ObjectivesManagerSignature,
                        IsObjectivesSignedByApprover = f.IsObjectivesSignedByApprover,
                        ObjectivesApproverSignature = f.ObjectivesApproverSignature,

                        // ResultsDefinition data block
                        IsResultsFreezed = f.IsResultsFreezed,
                        OverallKpi = f.OverallKpi,
                        IsProposalForBonusPayment = f.IsProposalForBonusPayment,
                        ManagerComment = f.ManagerComment,
                        EmployeeComment = f.EmployeeComment,
                        OtherComment = f.OtherComment,

                        // ResultsSignature data block
                        IsResultsSignedByEmployee = f.IsResultsSignedByEmployee,
                        ResultsEmployeeSignature = f.ResultsEmployeeSignature,
                        IsResultsRejectedByEmployee = f.IsResultsRejectedByEmployee,
                        IsResultsSignedByManager = f.IsResultsSignedByManager,
                        ResultsManagerSignature = f.ResultsManagerSignature,
                        IsResultsSignedByApprover = f.IsResultsSignedByApprover,
                        ResultsApproverSignature = f.ResultsApproverSignature,
                    })
                    .First();
        }

        public IQueryable<Form> GetFormsWithGlobalAccess(IEnumerable<FormGlobalAccess> formGlobalAccesses)
        {
            IQueryable<Form> formsQueryInitial = context.Forms.AsQueryable()
                .Include(f => f.Employee)
                    .ThenInclude(e => e.Department)
                .Include(f => f.Employee)
                    .ThenInclude(e => e.Team)
                .Include(f => f.Workproject)
                .Include(f => f.FormLocalAccess)
                .AsNoTracking();

            IQueryable<Form> formsQuery = null;

            foreach (var formGA in formGlobalAccesses)
            {
                IQueryable<Form> query = formsQueryInitial.Where(ExpressionBuilder.GetExpressionForGlobalAccess(formGA));

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
                    .ThenInclude(e => e.Department)
                .Include(f => f.Employee)
                    .ThenInclude(e => e.Team)
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
                    .ThenInclude(e => e.Department)
                .Include(f => f.Employee)
                    .ThenInclude(e => e.Team)
                .Include(f => f.Workproject)
                .Include(f => f.FormLocalAccess)
                .Where(ExpressionBuilder.GetExpressionForParticipation(userId))
                .AsNoTracking();
            return forms;
        }
    }
}
