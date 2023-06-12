using BonusSystemApplication.BLL.Infrastructure;
using BonusSystemApplication.DAL.Entities;
using BonusSystemApplication.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BonusSystemApplication.BLL.Processes.Promoting
{
    public class Promoter
    {
        private IFormRepository _formRepository;
        private IDefinitionRepository _definitionRepository;
        public Promoter(IFormRepository formRepository,
                        IDefinitionRepository definitionRepository)
        {
            _formRepository = formRepository;
            _definitionRepository = definitionRepository;
        }

        public Form GetPromotedForm(long initialFormId)
        {
            try
            {
                Form initialForm = _formRepository.GetFormForPromoting(initialFormId);
                if (initialForm == null)
                    throw new ValidationException("Unable to promote form. " +
                                                  "Selected form could not be found.");

                Periods period = initialForm.Definition.Period;
                int year = initialForm.Definition.Year;
                PrepareNextPeriodAndYear(ref period, ref year);

                if (year > DateTime.Now.Year + 1)
                    throw new ValidationException($"{initialForm.Definition.Employee.LastNameEng} {initialForm.Definition.Employee.FirstNameEng} for " +
                                                  $"{initialForm.Definition.Period} {initialForm.Definition.Year}. " +
                                                   "Unable to promote form. " +
                                                   "It's forbidden to save form with more than \u00B11 " +  // \u00B1: +- sign
                                                   "year in the past or future.");

                if (_definitionRepository.IsExistWithSamePropertyCombination(initialFormId,
                                                                             initialForm.Definition.EmployeeId,
                                                                             initialForm.Definition.WorkprojectId,
                                                                             year,
                                                                             period))
                {
                    throw new ValidationException($"{initialForm.Definition.Employee.LastNameEng} {initialForm.Definition.Employee.FirstNameEng} for " +
                                                  $"{initialForm.Definition.Period} {initialForm.Definition.Year}. " +
                                                   "Unable to promote form. " +
                                                   "Another form with same employee, workproject, next period or year is already exist.");
                }

                Form newForm = new Form()
                {
                    Id = 0,
                    AreObjectivesFrozen = false,
                    AreResultsFrozen = false,
                    Conclusion = new Conclusion(),
                    Signatures = new Signatures(),
                    Definition = new Definition(),
                    LocalAccesses = new List<LocalAccess>(),
                    ObjectivesResults = new List<ObjectiveResult>(),
                };

                newForm.Definition.Year = year;
                newForm.Definition.Period = period;
                newForm.Definition.IsWpmHox = initialForm.Definition.IsWpmHox;
                newForm.Definition.EmployeeId = initialForm.Definition.EmployeeId;
                newForm.Definition.ManagerId = initialForm.Definition.ManagerId;
                newForm.Definition.ApproverId = initialForm.Definition.ApproverId;
                newForm.Definition.WorkprojectId = initialForm.Definition.WorkprojectId;

                for (int i = 0; i < initialForm.ObjectivesResults.Count(); i++)
                {
                    ObjectiveResult objectiveResult = new ObjectiveResult
                    {
                        Row = initialForm.ObjectivesResults[i].Row,
                        Objective = initialForm.ObjectivesResults[i].Objective,
                        Result = new Result
                        {
                            KeyCheck = initialForm.ObjectivesResults[i].Result.KeyCheck,
                        }
                    };
                    newForm.ObjectivesResults.Add(objectiveResult);
                }

                for (int i = 0; i < initialForm.LocalAccesses.Count(); i++)
                {
                    LocalAccess localAccess = new LocalAccess()
                    {
                        UserId = initialForm.LocalAccesses[i].UserId,
                    };
                    newForm.LocalAccesses.Add(localAccess);
                }

                return newForm;
            }
            catch (Exception ex) when (ex is ArgumentNullException ||
                                       ex is DbUpdateException)
            {
                throw;
            }
            catch (ValidationException ex)
            {
                throw;
            }
        }

        private void PrepareNextPeriodAndYear(ref Periods period, ref int year)
        {
            switch(period)
            {
                case Periods.Q1:
                    period = Periods.Q2;
                    break;
                case Periods.Q2:
                    period = Periods.Q3;
                    break;
                case Periods.Q3:
                    period = Periods.Q4;
                    break;
                case Periods.Q4:
                    period = Periods.Q1;
                    year = year + 1;
                    break;
                case Periods.S1:
                    period = Periods.S2;
                    break;
                case Periods.S2:
                    period = Periods.S1;
                    year = year + 1;
                    break;
                case Periods.Y:
                    period = Periods.Y;
                    year = year + 1;
                    break;
                default:
                    period = Periods.Q1;
                    break;
            }
        }
    }
}
