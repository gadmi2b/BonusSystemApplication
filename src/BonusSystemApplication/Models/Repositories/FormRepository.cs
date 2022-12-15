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
            originalForm.Signatures = changedForm.Signatures;

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
                        Id = f.Id,
                        IsObjectivesFreezed = f.IsObjectivesFreezed,
                        IsResultsFreezed = f.IsResultsFreezed,

                        // Definition data block
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

                        // Conclusion data block
                        OverallKpi = f.OverallKpi,
                        IsProposalForBonusPayment = f.IsProposalForBonusPayment,
                        ManagerComment = f.ManagerComment,
                        EmployeeComment = f.EmployeeComment,
                        OtherComment = f.OtherComment,

                        // Objectives and Results data block
                        ObjectivesResults = f.ObjectivesResults,

                        // Signatures data block
                        Signatures = f.Signatures,
                    })
                    .First();
        }

        public Form GetIsFreezedAndSignatureData(long formId)
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
                        Signatures = f.Signatures,
                    })
                    .First();
            return form;
        }
        public Form GetObjectivesResultsData(long formId)
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


        public IQueryable<Form> GetDefinition(long formId)
        {
            IQueryable<Form> formQuery = context.Forms.AsQueryable()
                .Where(f => f.Id == formId)
                .Select(f => new Form
                {
                    Id = f.Id,
                    IsObjectivesFreezed = f.IsObjectivesFreezed,
                    IsResultsFreezed = f.IsResultsFreezed,
                    Period = f.Period,
                    Year = f.Year,
                    IsWpmHox = f.IsWpmHox,
                    EmployeeId = f.EmployeeId,
                    ManagerId = f.ManagerId,
                    ApproverId = f.ApproverId,
                    WorkprojectId = f.WorkprojectId,
                });
            return formQuery;
        }
        public IQueryable<Form> GetObjectives(long formId)
        {
            IQueryable<Form> formQuery = context.Forms.AsQueryable()
                .Where(f => f.Id == formId)
                .Select(f => new Form
                {
                    Id = f.Id,
                    ObjectivesResults = f.ObjectivesResults.AsQueryable()
                        .Select(or => new ObjectiveResult
                        {
                            Id = or.Id,
                            Row = or.Row,
                            Objective = or.Objective,
                        })
                        .ToList(),
                });
            return formQuery;
        }
        public IQueryable<Form> GetResults(long formId)
        {
            IQueryable<Form> formQuery = context.Forms.AsQueryable()
                .Where(f => f.Id == formId)
                .Select(f => new Form
                {
                    Id = f.Id,
                    ObjectivesResults = f.ObjectivesResults.AsQueryable()
                        .Select(or => new ObjectiveResult
                        {
                            Id = or.Id,
                            Row = or.Row,
                            Result = or.Result,
                        })
                        .ToList(),
                });
            return formQuery;
        }
        public IQueryable<Form> GetConclusion(long formId)
        {
            IQueryable<Form> formQuery = context.Forms.AsQueryable()
                .Where(f => f.Id == formId)
                .Select(f => new Form
                {
                    Id = f.Id,
                    OverallKpi = f.OverallKpi,
                    IsProposalForBonusPayment = f.IsProposalForBonusPayment,
                    ManagerComment = f.ManagerComment,
                    EmployeeComment = f.EmployeeComment,
                    OtherComment = f.OtherComment,
                });
            return formQuery;
        }
    }
}
