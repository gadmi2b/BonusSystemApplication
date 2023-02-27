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

        public List<Form> GetForms(List<long> formIds)
        {
            return context.Forms.TagWith($"Forms data for Index view: {formIds.Count()} forms total")
                    .Where(f => formIds.Contains(f.Id))
                    .Select(f => new Form
                    {
                        Id = f.Id,
                        LastSavedDateTime = f.LastSavedDateTime,
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
        public Form GetIsFreezedStates(long formId)
        {
            return context.Forms.TagWith("Form IsFreezedStates requesting")
                    .Where(f => f.Id == formId)
                    .Select(f => new Form
                    {
                        Id = f.Id,
                        IsObjectivesFreezed = f.IsObjectivesFreezed,
                        IsResultsFreezed = f.IsResultsFreezed,
                    })
                    .First();
        }

        public Form GetForm(long formId) //OK
        {
            return context.Forms.TagWith("Form data for Form view requesting")
                    .Where(f => f.Id == formId)
                    .Select(f => new Form
                    {
                        Id = f.Id,
                        IsObjectivesFreezed = f.IsObjectivesFreezed,
                        IsResultsFreezed = f.IsResultsFreezed,

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



        public void CreateForm(Form form)
        {
            throw new NotImplementedException();
        }

        public void UpdateFormSignatures(Form changedForm)
        {
            Form originalForm = context.Forms.Find(changedForm.Id);
            if (originalForm == null)
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


        public Form GetIsFreezedAndSignatures(long formId) //OK
        {
            Form form = context.Forms.TagWith("IsFreezed and Signatures requesting")
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

        public List<long> GetFormIdsWhereLocalAccess(long userId)
        {
            return context.Forms
                .TagWith("Requesting form Ids where user has local access")
                .Where(f => f.LocalAccesses.Any(la => la.UserId == userId))
                .Select(f => f.Id)
                .ToList();

        }
    }
}
