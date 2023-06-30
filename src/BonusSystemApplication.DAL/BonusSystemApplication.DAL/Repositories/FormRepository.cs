using BonusSystemApplication.DAL.Interfaces;
using BonusSystemApplication.DAL.Entities;
using BonusSystemApplication.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace BonusSystemApplication.DAL.Repositories
{
    public class FormRepository : IFormRepository
    {
        private DataContext _context;
        public FormRepository(DataContext ctx) => _context = ctx;

        public async Task<Form> GetFormAsync(long formId)
        {
            return await _context.Forms.AsNoTracking()
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
                    .FirstAsync();
        }
        public async Task<Form> GetFormForPromotingAsync(long formId)
        {
            return await _context.Forms.AsNoTracking()
                    .Where(f => f.Id == formId)
                    .Include(f => f.Definition)
                        .ThenInclude(d => d.Employee)
                    .Include(f => f.ObjectivesResults)
                    .Include(f => f.LocalAccesses)
                    .FirstAsync();
        }
        public async Task<List<Form>> GetFormsAsync(List<long> formIds)
        {
            return await _context.Forms.AsNoTracking()
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
                                    Name = f.Definition.Employee.Team == null ? string.Empty : f.Definition.Employee.Team.Name,
                                },
                                Department = new Department
                                {
                                    Name = f.Definition.Employee.Department == null ? string.Empty : f.Definition.Employee.Department.Name,
                                },
                            },
                            Workproject = new Workproject
                            {
                                Id = f.Definition.Workproject.Id,
                                Name = f.Definition.Workproject.Name,
                            },
                        }
                    })
                    .ToListAsync();
        }
        public async Task<Form> GetStatesAsync(long formId)
        {
            return await _context.Forms.AsNoTracking()
                    .Where(f => f.Id == formId)
                    .Select(f => new Form
                    {
                        Id = f.Id,
                        AreObjectivesFrozen = f.AreObjectivesFrozen,
                        AreResultsFrozen = f.AreResultsFrozen,
                    })
                    .FirstAsync();
        }
        public async Task<Form> GetStatesAndSignaturesAsync(long formId)
        {
            Form form = await _context.Forms.AsNoTracking()
                    .Where(f => f.Id == formId)
                    .Select(f => new Form
                    {
                        Id = f.Id,
                        AreObjectivesFrozen = f.AreObjectivesFrozen,
                        AreResultsFrozen = f.AreResultsFrozen,
                        Signatures = f.Signatures,
                    })
                    .FirstAsync();
            return form;
        }
        public async Task<List<long>> GetFormIdsWhereLocalAccessAsync(long userId)
        {
            return await _context.Forms.AsNoTracking()
                    .Where(f => f.LocalAccesses.Any(la => la.UserId == userId))
                    .Select(f => f.Id)
                    .ToListAsync();
        }

        public async Task CreateFormAsync(Form newForm)
        {
            await _context.Forms.AddAsync(newForm);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStatesAsync(Form changedForm)
        {
            Form? originalForm = await _context.Forms.FindAsync(changedForm.Id);

            ArgumentNullException.ThrowIfNull(originalForm, nameof(originalForm));

            originalForm!.AreObjectivesFrozen = changedForm.AreObjectivesFrozen;
            originalForm!.AreResultsFrozen = changedForm.AreResultsFrozen;
            originalForm!.LastSavedAt = changedForm.LastSavedAt;
            originalForm!.LastSavedBy = changedForm.LastSavedBy;

            await _context.SaveChangesAsync();
        }
        public async Task UpdateSignaturesAsync(Form changedForm)
        {
            Form? originalForm = await _context.Forms
                        .Include(f => f.Signatures)
                        .Where(f => f.Id == changedForm.Id)
                        .FirstAsync();

            ArgumentNullException.ThrowIfNull(originalForm, nameof(originalForm));

            originalForm.LastSavedBy = changedForm.LastSavedBy;
            originalForm.LastSavedAt = changedForm.LastSavedAt;
            originalForm.Signatures = changedForm.Signatures;

            await _context.SaveChangesAsync();
        }
        public async Task UpdateConclusionCommentsAsync(Form changedForm)
        {
            Form? originalForm = await _context.Forms
                        .Include(f => f.Conclusion)
                        .Where(f => f.Id == changedForm.Id)
                        .FirstAsync();

            ArgumentNullException.ThrowIfNull(originalForm, nameof(originalForm));

            originalForm.LastSavedBy = changedForm.LastSavedBy;
            originalForm.LastSavedAt = changedForm.LastSavedAt;
            originalForm.Conclusion.ManagerComment = changedForm.Conclusion.ManagerComment;
            originalForm.Conclusion.EmployeeComment = changedForm.Conclusion.EmployeeComment;
            originalForm.Conclusion.OtherComment = changedForm.Conclusion.OtherComment;

            await _context.SaveChangesAsync();
        }
        public async Task UpdateResultsConclusionAsync(Form changedForm)
        {
            Form? originalForm = await _context.Forms
                        .Include(f => f.Conclusion)
                        .Include(f => f.ObjectivesResults)
                        .Where(f => f.Id == changedForm.Id)
                        .FirstAsync();

            ArgumentNullException.ThrowIfNull(originalForm, nameof(originalForm));

            originalForm.LastSavedBy = changedForm.LastSavedBy;
            originalForm.LastSavedAt = changedForm.LastSavedAt;
            originalForm.Conclusion = changedForm.Conclusion;
            for(int index = 0; index < changedForm.ObjectivesResults.Count; index++)
            {
                originalForm.ObjectivesResults[index].Result = changedForm.ObjectivesResults[index].Result;
            }

            await _context.SaveChangesAsync();
        }
        public async Task UpdateDefinitionObjectivesResultsConclusionAsync(Form changedForm)
        {
            Form? originalForm = await _context.Forms
                        .Include(f => f.Definition)
                        .Include(f => f.Conclusion)
                        .Include(f => f.ObjectivesResults)
                        .Where(f => f.Id == changedForm.Id)
                        .FirstAsync();

            ArgumentNullException.ThrowIfNull(originalForm, nameof(originalForm));

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

            await _context.SaveChangesAsync();
        }

        public async Task DeleteFormAsync(long formId)
        {
            Form? form = await _context.Forms
                        .FirstAsync(f => f.Id == formId);
            if (form != null)
            {
                _context.Forms.Remove(form);
                await _context.SaveChangesAsync();
            }
        }
    }
}
