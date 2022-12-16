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

        public Form GetFormData(long formId) //OK
        {
            return context.Forms
                    .Where(f => f.Id == formId)
                    .Select(f => new Form
                    {
                        Id = f.Id,
                        IsObjectivesFreezed = f.IsObjectivesFreezed,
                        IsResultsFreezed = f.IsResultsFreezed,

                        //Definition data block
                        Definition = new Definition
                        {
                            Employee = new User
                            {
                                Id = f.Definition.Employee.Id,
                                FirstNameEng = f.Definition.Employee.FirstNameEng,
                                LastNameEng = f.Definition.Employee.LastNameEng,
                                Pid = f.Definition.Employee.Pid,
                                Team = new Team
                                {
                                    Name = f.Definition.Employee.Team == null ? string.Empty : f.Definition.Employee.Team.Name,
                                },
                                Position = new Position
                                {
                                    NameEng = f.Definition.Employee.Position == null ? string.Empty : f.Definition.Employee.Position.NameEng,
                                },
                            },
                            Manager = new User
                            {
                                Id = f.Definition.ManagerId == null ? 0 : (long)f.Definition.ManagerId,
                                FirstNameEng = f.Definition.Employee.FirstNameEng,
                                LastNameEng = f.Definition.Employee.LastNameEng,
                            },
                            Approver = new User
                            {
                                Id = f.Definition.ApproverId == null ? 0 : (long)f.Definition.ApproverId,
                                FirstNameEng = f.Definition.Employee.FirstNameEng,
                                LastNameEng = f.Definition.Employee.LastNameEng,
                            },
                            Workproject = new Workproject
                            {
                                Id = f.Definition.WorkprojectId == null ? 0 : (long)f.Definition.WorkprojectId,
                                Name = f.Definition.Workproject == null ? string.Empty : f.Definition.Workproject.Name,
                                Description = f.Definition.Workproject == null ? string.Empty : f.Definition.Workproject.Description,
                            },
                            Period = f.Definition.Period,
                            Year = f.Definition.Year,
                            IsWpmHox = f.Definition.IsWpmHox,
                        },

                        //Conclusion data block
                        Conclusion = f.Conclusion,

                        // Objectives and Results data block
                        ObjectivesResults = f.ObjectivesResults,

                        // Signatures data block
                        Signatures = f.Signatures,
                    })
                    .First();
        }

        public Form GetIsFreezedAndSignatureData(long formId) //OK
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
        public Form GetObjectivesResultsData(long formId) //OK
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

        public IQueryable<Form> GetAllFormsQuery(IEnumerable<GlobalAccess> globalAccesses, long userId)
        {
            IQueryable<Form> allFormsQuery= context.Forms.AsQueryable()
                .Include(f => f.Definition)
                    .ThenInclude(d => d.Employee)
                        .ThenInclude(e => e.Department)
                .Include(f => f.Definition)
                    .ThenInclude(d => d.Employee)
                        .ThenInclude(e => e.Team)
                .Include(f => f.Definition)
                    .ThenInclude(f => f.Workproject)
                .Include(f => f.LocalAccesses)
                .AsNoTracking();

            IQueryable<Form> formsQuery1 = allFormsQuery.Where(ExpressionBuilder.GetExpressionForLocalAccess(userId));
            IQueryable<Form> formsQuery2 = allFormsQuery.Where(ExpressionBuilder.GetExpressionForParticipation(userId));

            IQueryable<Form> formsQuery = formsQuery1.Union(formsQuery2);
            List<Form> forms = formsQuery.ToList();

            foreach (var formGA in globalAccesses)
            {
                IQueryable<Form> query = allFormsQuery.Where(ExpressionBuilder.GetExpressionForGlobalAccess(formGA));

                if (query == null)
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

            //formsQuery.Where(ExpressionBuilder.GetExpressionForLocalAccess(userId));
            //formsQuery.Where(ExpressionBuilder.GetExpressionForParticipation(userId));

            return formsQuery;
        }

        public IQueryable<Form> GetFormsWithGlobalAccess(IEnumerable<GlobalAccess> globalAccesses) //OK
        {
            IQueryable<Form> formsQueryInitial = context.Forms.AsQueryable()
                .Include(f => f.Definition)
                    .ThenInclude(d => d.Employee)
                        .ThenInclude(e => e.Department)
                .Include(f => f.Definition)
                    .ThenInclude(d => d.Employee)
                        .ThenInclude(e => e.Team)
                .Include(f => f.Definition)
                    .ThenInclude(f => f.Workproject)
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
        public IQueryable<Form> GetFormsWithLocalAccess(long userId) //OK
        {
            IQueryable<Form> forms = context.Forms.AsQueryable()
                .Include(f => f.Definition)
                    .ThenInclude(d => d.Employee)
                        .ThenInclude(e => e.Department)
                .Include(f => f.Definition)
                    .ThenInclude(d => d.Employee)
                        .ThenInclude(e => e.Team)
                .Include(f => f.Definition)
                    .ThenInclude(f => f.Workproject)
                .Include(f => f.LocalAccesses)
                .Where(ExpressionBuilder.GetExpressionForLocalAccess(userId))
                .AsNoTracking();
            return forms;
        }
        public IQueryable<Form> GetFormsWithParticipation(long userId) //OK
        {
            IQueryable<Form> forms = context.Forms.AsQueryable()
                .Include(f => f.Definition)
                    .ThenInclude(d => d.Employee)
                        .ThenInclude(e => e.Department)
                .Include(f => f.Definition)
                    .ThenInclude(d => d.Employee)
                        .ThenInclude(e => e.Team)
                .Include(f => f.Definition)
                    .ThenInclude(f => f.Workproject)
                .Include(f => f.LocalAccesses)
                .Where(ExpressionBuilder.GetExpressionForParticipation(userId))
                .AsNoTracking();
            return forms;
        }

        /*
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
        */
    }
}
