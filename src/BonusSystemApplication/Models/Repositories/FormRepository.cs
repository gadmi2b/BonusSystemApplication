using BonusSystemApplication.Models.ViewModels.FormViewModel;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient.Server;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

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

        public void UpdateFormSignatures(Form changedForm)
        {
            Form originalForm = context.Forms.Find(changedForm.Id);
            if(originalForm == null)
            {
                throw new ArgumentNullException();
            }

            originalForm.LastSavedBy = changedForm.LastSavedBy;
            originalForm.LastSavedDateTime = changedForm.LastSavedDateTime;

            originalForm.IsObjectivesSignedByEmployee = changedForm.IsObjectivesSignedByEmployee;
            originalForm.ObjectivesEmployeeSignature = changedForm.ObjectivesEmployeeSignature;
            originalForm.IsObjectivesRejectedByEmployee = changedForm.IsObjectivesRejectedByEmployee;
            originalForm.IsObjectivesSignedByManager = changedForm.IsObjectivesSignedByManager;
            originalForm.ObjectivesManagerSignature = changedForm.ObjectivesManagerSignature;
            originalForm.IsObjectivesSignedByApprover = changedForm.IsObjectivesSignedByApprover;
            originalForm.ObjectivesApproverSignature = changedForm.ObjectivesApproverSignature;

            originalForm.IsResultsSignedByEmployee = changedForm.IsResultsSignedByEmployee;
            originalForm.ResultsEmployeeSignature = changedForm.ResultsEmployeeSignature;
            originalForm.IsResultsRejectedByEmployee = changedForm.IsResultsRejectedByEmployee;
            originalForm.IsResultsSignedByManager = changedForm.IsResultsSignedByManager;
            originalForm.ResultsManagerSignature = changedForm.ResultsManagerSignature;
            originalForm.IsResultsSignedByApprover = changedForm.IsResultsSignedByApprover;
            originalForm.ResultsApproverSignature = changedForm.ResultsApproverSignature;

            context.SaveChanges();
        }

        public void UpdateFormObjectivesResults(Form changedForm)
        {
            Form originalForm = context.Forms.Find(changedForm.Id);
            if (originalForm == null)
            {
                throw new ArgumentNullException();
            }
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
                        // Definition data block
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

                        // Conclusion data block
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

        public Form GetFormSignatureData(long formId)
        {
            Form form = context.Forms
                    .Where(f => f.Id == formId)
                    .Select(f => new Form
                    {
                        Id = f.Id,
                        IsObjectivesFreezed = f.IsObjectivesFreezed,
                        IsResultsFreezed = f.IsResultsFreezed,
                        LastSavedBy = f.LastSavedBy,
                        LastSavedDateTime = f.LastSavedDateTime,

                        IsObjectivesSignedByEmployee = f.IsObjectivesSignedByEmployee,
                        ObjectivesEmployeeSignature = f.ObjectivesEmployeeSignature,
                        IsObjectivesRejectedByEmployee = f.IsObjectivesRejectedByEmployee,
                        IsObjectivesSignedByManager = f.IsObjectivesSignedByManager,
                        ObjectivesManagerSignature = f.ObjectivesManagerSignature,
                        IsObjectivesSignedByApprover = f.IsObjectivesSignedByApprover,
                        ObjectivesApproverSignature = f.ObjectivesApproverSignature,

                        IsResultsSignedByEmployee = f.IsResultsSignedByEmployee,
                        ResultsEmployeeSignature = f.ResultsEmployeeSignature,
                        IsResultsRejectedByEmployee = f.IsResultsRejectedByEmployee,
                        IsResultsSignedByManager = f.IsResultsSignedByManager,
                        ResultsManagerSignature = f.ResultsManagerSignature,
                        IsResultsSignedByApprover = f.IsResultsSignedByApprover,
                        ResultsApproverSignature = f.ResultsApproverSignature,
                    })
                    .First();
            return form;
        }
        public Form GetFormIsFreezedStates(long formId)
        {
            return context.Forms
                    .Where(f => f.Id == formId)
                    .Select(f => new Form()
                    {
                        Id = f.Id,
                        IsObjectivesFreezed = f.IsObjectivesFreezed,
                        IsResultsFreezed = f.IsResultsFreezed,
                    })
                    .First();
        }
        public Form GetFormObjectivesResultsData(long formId)
        {
            Form form = context.Forms
                    .Where(f => f.Id == formId)
                    .Select(f => new Form
                    {
                        Id = f.Id,
                        IsObjectivesFreezed = f.IsObjectivesFreezed,
                        IsResultsFreezed = f.IsResultsFreezed,
                        LastSavedBy = f.LastSavedBy,
                        LastSavedDateTime = f.LastSavedDateTime,

                        ObjectivesResults = f.ObjectivesResults,
                    })
                    .First();
            return form;
        }
        public IQueryable<Form> GetFormsWithGlobalAccess(IEnumerable<GlobalAccess> globalAccesses)
        {
            IQueryable<Form> formsQueryInitial = context.Forms.AsQueryable()
                .Include(f => f.Employee)
                    .ThenInclude(e => e.Department)
                .Include(f => f.Employee)
                    .ThenInclude(e => e.Team)
                .Include(f => f.Workproject)
                .Include(f => f.LocalAccesses)
                .AsNoTracking();

            IQueryable<Form> formsQuery = null;

            foreach (var formGA in globalAccesses)
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
                .Include(f => f.LocalAccesses)
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
                .Include(f => f.LocalAccesses)
                .Where(ExpressionBuilder.GetExpressionForParticipation(userId))
                .AsNoTracking();
            return forms;
        }
    }
}
