using BonusSystemApplication.DAL.Interfaces;
using BonusSystemApplication.DAL.Entities;
using BonusSystemApplication.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace BonusSystemApplication.DAL.Repositories
{
    public class FormRepository : IFormRepository
    {
        private DataContext context;
        public FormRepository(DataContext ctx) => context = ctx;

        public Form GetForm(long formId) //OK
        {
            return context.Forms.TagWith("Requesting form data")
                    .Where(f => f.Id == formId)
                    .Select(f => new Form
                    {
                        Id = f.Id,
                        AreObjectivesFrozen = f.AreObjectivesFrozen,
                        AreResultsFrozen = f.AreResultsFrozen,

                        //Definition data block
                        Definition = new Definition
                        {
                            EmployeeId = f.Definition.EmployeeId,
                            ManagerId = f.Definition.ManagerId,
                            ApproverId = f.Definition.ApproverId,
                            WorkprojectId = f.Definition.WorkprojectId,
                            Period = f.Definition.Period,
                            Year = f.Definition.Year,
                            IsWpmHox = f.Definition.IsWpmHox,

                            Employee = new User
                            {
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
                                FirstNameEng = f.Definition.Manager == null ? string.Empty : f.Definition.Manager.FirstNameEng,
                                LastNameEng = f.Definition.Manager == null ? string.Empty : f.Definition.Manager.LastNameEng,
                            },
                            Approver = new User
                            {
                                FirstNameEng = f.Definition.Approver == null ? string.Empty : f.Definition.Approver.FirstNameEng,
                                LastNameEng = f.Definition.Approver == null ? string.Empty : f.Definition.Approver.LastNameEng,
                            },
                            Workproject = new Workproject
                            {
                                Name = f.Definition.Workproject == null ? string.Empty : f.Definition.Workproject.Name,
                                Description = f.Definition.Workproject == null ? string.Empty : f.Definition.Workproject.Description,
                            },
                        },
                        // Objectives and Results data block
                        ObjectivesResults = f.ObjectivesResults,
                        // Conclusion data block
                        Conclusion = f.Conclusion,
                        // Signatures data block
                        Signatures = f.Signatures,
                    })
                    .First();
        }
        public List<Form> GetForms(List<long> formIds)
        {
            return context.Forms.TagWith($"Forms data for Index view: {formIds.Count()} forms total")
                    .Where(f => formIds.Contains(f.Id))
                    .Select(f => new Form
                    {
                        Id = f.Id,
                        LastSavedAt = f.LastSavedAt,
                        Definition = new Definition
                        {
                            Period = f.Definition.Period,
                            Year = f.Definition.Year,
                            ApproverId = f.Definition.ApproverId,
                            ManagerId = f.Definition.ManagerId,
                            EmployeeId = f.Definition.EmployeeId,

                            Employee = new User
                            {
                                LastNameEng = f.Definition.Employee.LastNameEng,
                                FirstNameEng = f.Definition.Employee.FirstNameEng,
                                Team = new Team
                                {
                                    Name = f.Definition.Employee.Team.Name,
                                },
                                Department = new Department
                                {
                                    Name = f.Definition.Employee.Department.Name,
                                },
                            },
                            Workproject = new Workproject
                            {
                                Id = f.Definition.Workproject.Id,
                                Name = f.Definition.Workproject.Name,
                            },
                        }
                    })
                    .ToList();
        }
        public Form GetStates(long formId)
        {
            return context.Forms.TagWith("Form IsFrozen states requesting")
                    .Where(f => f.Id == formId)
                    .Select(f => new Form
                    {
                        Id = f.Id,
                        AreObjectivesFrozen = f.AreObjectivesFrozen,
                        AreResultsFrozen = f.AreResultsFrozen,
                    })
                    .First();
        }
        public Form GetStatesAndSignatures(long formId) //OK
        {
            Form form = context.Forms.TagWith("IsFrozen and Signatures requesting")
                    .Where(f => f.Id == formId)
                    .Select(f => new Form
                    {
                        Id = f.Id,
                        AreObjectivesFrozen = f.AreObjectivesFrozen,
                        AreResultsFrozen = f.AreResultsFrozen,
                        Signatures = f.Signatures,
                    })
                    .First();
            return form;
        }
        public List<long> GetFormIdsWhereLocalAccess(long userId)
        {
            return context.Forms
                    .TagWith("Requesting form Ids where user has local access")
                    .Where(f => f.LocalAccesses.Any(la => la.UserId == userId))
                    .Select(f => f.Id)
                    .ToList();

        }

        public void CreateForm(Form form)
        {
            context.Forms.Add(form);
            context.SaveChanges();
        }

        public void UpdateStates(Form form)
        {
            Form originalForm = context.Forms.TagWith("Form requesting")
                    .Where(f => f.Id == form.Id)
                    .First();   

            if (originalForm == null)
            {
                throw new ArgumentNullException();
            }

            originalForm!.AreObjectivesFrozen = form.AreObjectivesFrozen;
            originalForm!.AreResultsFrozen = form.AreResultsFrozen;
            originalForm!.LastSavedAt = form.LastSavedAt;
            originalForm!.LastSavedBy = form.LastSavedBy;

            context.SaveChanges();
        }

        public void UpdateSignatures(Form changedForm)
        {
            Form originalForm = context.Forms.Find(changedForm.Id);
            if (originalForm == null)
            {
                throw new ArgumentNullException();
            }

            originalForm.LastSavedBy = changedForm.LastSavedBy;
            originalForm.LastSavedAt = changedForm.LastSavedAt;
            originalForm.Signatures = changedForm.Signatures;

            context.SaveChanges();
        }

        public void UpdateConclusionComments(Form changedForm)
        {
            Form? originalForm = context.Forms
                        .Include(f => f.Conclusion)
                        .Where(f => f.Id == changedForm.Id)
                        .First();

            if (originalForm == null)
            {
                throw new ArgumentNullException();
            }

            originalForm.LastSavedBy = changedForm.LastSavedBy;
            originalForm.LastSavedAt = changedForm.LastSavedAt;
            originalForm.Conclusion.ManagerComment = changedForm.Conclusion.ManagerComment;
            originalForm.Conclusion.EmployeeComment = changedForm.Conclusion.EmployeeComment;
            originalForm.Conclusion.OtherComment = changedForm.Conclusion.OtherComment;

            context.SaveChanges();
        }

        public void UpdateResultsConclusion(Form changedForm)
        {
            Form? originalForm = context.Forms
                        .Include(f => f.Conclusion)
                        .Include(f => f.ObjectivesResults)
                        .Where(f => f.Id == changedForm.Id)
                        .First();

            if (originalForm == null)
            {
                throw new ArgumentNullException();
            }

            originalForm.LastSavedBy = changedForm.LastSavedBy;
            originalForm.LastSavedAt = changedForm.LastSavedAt;

            originalForm.Conclusion = changedForm.Conclusion;
            for(int index = 0; index < changedForm.ObjectivesResults.Count; index++)
            {
                originalForm.ObjectivesResults[index].Result = changedForm.ObjectivesResults[index].Result;
            }

            context.SaveChanges();
        }
        public void UpdateDefinitionObjectivesResultsConclusion(Form changedForm)
        {
            // TODO: add AsNoTracking to all requests to avoid .Signatures to be loaded
            Form? originalForm = context.Forms
                        .Include(f => f.Definition)
                        .Include(f => f.Conclusion)
                        .Include(f => f.ObjectivesResults)
                        .Where(f => f.Id == changedForm.Id)
                        .First();

            if (originalForm == null)
            {
                throw new ArgumentNullException();
            }

            originalForm.LastSavedBy = changedForm.LastSavedBy;
            originalForm.LastSavedAt = changedForm.LastSavedAt;

            originalForm.Definition.Year = changedForm.Definition.Year;
            originalForm.Definition.Period = changedForm.Definition.Period;
            originalForm.Definition.IsWpmHox = changedForm.Definition.IsWpmHox;
            originalForm.Definition.ManagerId = changedForm.Definition.ManagerId;
            originalForm.Definition.ApproverId = changedForm.Definition.ApproverId;
            originalForm.Definition.EmployeeId = changedForm.Definition.EmployeeId;
            originalForm.Definition.WorkprojectId = changedForm.Definition.WorkprojectId;

            originalForm.Conclusion = changedForm.Conclusion;
            originalForm.ObjectivesResults = changedForm.ObjectivesResults;

            context.SaveChanges();
        }

        public void DeleteForm(long id)
        {
            throw new NotImplementedException();
        }

        public IList<ObjectiveResult> GetObjectivesResults(long formId)
        {
            return context.Forms.TagWith($"Get Definition for FormId: {formId}")
                    .Where(f => f.Id == formId)
                    .Select(f => f.ObjectivesResults)
                    .First();
        }
        public IList<ObjectiveResult> GetObjectives(long formId)
        {
            return (IList<ObjectiveResult>)context.Forms.TagWith($"Get Objectives for FormId: {formId}")
                    .Where(f => f.Id == formId)
                    .Select(f => f.ObjectivesResults.AsQueryable()
                        .Select(or => new ObjectiveResult
                        {
                            Row = or.Row,
                            Objective = or.Objective
                        }))
                        .ToList();
        }
        public IList<ObjectiveResult> GetResults(long formId)
        {
            return (IList<ObjectiveResult>)context.Forms.TagWith($"Get Results for FormId: {formId}")
                    .Where(f => f.Id == formId)
                    .Select(f => f.ObjectivesResults.AsQueryable()
                        .Select(or => new ObjectiveResult
                        {
                            Row = or.Row,
                            Result = or.Result
                        }))
                        .ToList();
        }
        public Conclusion GetConclustion(long formId)
        {
            return context.Forms.TagWith($"Get Conclusion for FormId: {formId}")
                    .Where(f => f.Id == formId)
                    .Select(f => new Conclusion
                    {
                        OverallKpi = f.Conclusion.OverallKpi,
                        IsProposalForBonusPayment = f.Conclusion.IsProposalForBonusPayment,
                        ManagerComment = f.Conclusion.ManagerComment,
                        EmployeeComment = f.Conclusion.EmployeeComment,
                        OtherComment = f.Conclusion.OtherComment,
                    })
                    .First();
        }
        public Definition GetDefinition(long formId)
        {
            return context.Forms.TagWith($"Get Definition for FormId: {formId}")
                    .Where(f => f.Id == formId)
                    .Select(f => new Definition
                    {
                        EmployeeId = f.Definition.EmployeeId,
                        ManagerId = f.Definition.ManagerId,
                        ApproverId = f.Definition.ApproverId,
                        WorkprojectId = f.Definition.WorkprojectId,
                        Period = f.Definition.Period,
                        Year = f.Definition.Year,
                        IsWpmHox = f.Definition.IsWpmHox,

                        Employee = new User
                        {
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
                            FirstNameEng = f.Definition.Manager == null ? string.Empty : f.Definition.Manager.FirstNameEng,
                            LastNameEng = f.Definition.Manager == null ? string.Empty : f.Definition.Manager.LastNameEng,
                        },
                        Approver = new User
                        {
                            FirstNameEng = f.Definition.Approver == null ? string.Empty : f.Definition.Approver.FirstNameEng,
                            LastNameEng = f.Definition.Approver == null ? string.Empty : f.Definition.Approver.LastNameEng,
                        },
                        Workproject = new Workproject
                        {
                            Name = f.Definition.Workproject == null ? string.Empty : f.Definition.Workproject.Name,
                            Description = f.Definition.Workproject == null ? string.Empty : f.Definition.Workproject.Description,
                        },
                    })
                    .First();
        }

    }
}
