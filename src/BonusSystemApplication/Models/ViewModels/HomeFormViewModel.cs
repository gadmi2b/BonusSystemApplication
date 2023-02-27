using BonusSystemApplication.DAL.Entities;
using BonusSystemApplication.Models.Forms.Edit;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BonusSystemApplication.Models.ViewModels
{
    public class HomeFormViewModel //Controller name | Action name | ViewModel
    {
        public DefinitionVM Definition { get; set; }
        public ConclusionVM Conclusion { get; set; }
        public SignaturesVM Signatures { get; set; }
        public IList<ObjectiveResultVM> ObjectivesResults { get; set; }

        public List<SelectListItem> PeriodSelectList { get; set; }
        public List<SelectListItem> EmployeeSelectList { get; set; }
        public List<SelectListItem> WorkprojectSelectList { get; set; }

        public long Id { get; set; }
        public bool IsObjectivesFreezed { get; set; }
        public bool IsResultsFreezed { get; set; }

        public HomeFormViewModel() { }

        public HomeFormViewModel(IQueryable<User> usersQuery,
                                 IQueryable<Workproject> workprojectsQuery)
        {
            Id = 0;
            Definition = new DefinitionVM();
            Conclusion = new ConclusionVM();
            Signatures = new SignaturesVM
            {
                ForObjectives = new ForObjectives(),
                ForResults = new ForResults(),
            };

            List<ObjectiveResultVM> objectivesResults = new List<ObjectiveResultVM>();
            for (int i = 0; i < 10; i++)
            {
                ObjectiveResultVM objectiveResult = new ObjectiveResultVM()
                {
                    Row = i + 1,
                    Objective = new Objective(),
                    Result = new Result(),
                };
                objectivesResults.Add(objectiveResult);
            }
            ObjectivesResults = objectivesResults;
            IsObjectivesFreezed = false;
            IsResultsFreezed = false;

            InitializeDropdowns(usersQuery, workprojectsQuery);
        }

        public HomeFormViewModel(IQueryable<User> usersQuery,
                                 IQueryable<Workproject> workprojectsQuery,
                                 Form form)
        {
            Id = form.Id;
            IsObjectivesFreezed = form.IsObjectivesFreezed;
            IsResultsFreezed = form.IsResultsFreezed;

            Definition = new DefinitionVM(form.Definition);
            Conclusion = new ConclusionVM(form.Conclusion);
            Signatures = new SignaturesVM(form.Signatures);

            foreach (var objRes in form.ObjectivesResults)
            {
                ObjectiveResultVM objResViewModel = new ObjectiveResultVM(objRes);
                ObjectivesResults.Add(objResViewModel);
            }

            InitializeDropdowns(usersQuery, workprojectsQuery);
        }


        public void InitializeDropdowns(IQueryable<User> usersQuery, IQueryable<Workproject> workprojectsQuery)
        {
            PeriodSelectList = Enum.GetNames(typeof(Periods))
                    .Select(s => new SelectListItem
                    {
                        Value = s,
                        Text = s,
                    })
                    .ToList();
            EmployeeSelectList = usersQuery
                    .Select(u => new SelectListItem
                    {
                        Value = u.Id.ToString(),
                        Text = $"{u.LastNameEng} {u.FirstNameEng}",
                    })
                    .ToList();
            WorkprojectSelectList = workprojectsQuery
                    .Select(w => new SelectListItem
                    {
                        Value = w.Id.ToString(),
                        Text = w.Name,
                    })
                    .ToList();
        }
        
        public void InitilizeStatesAndSignatures(Form? statesAndSignatures)
        {
            if(statesAndSignatures == null)
            {
                Signatures = new SignaturesVM
                {
                    ForObjectives = new ForObjectives(),
                    ForResults = new ForResults(),
                };
                IsObjectivesFreezed = false;
                IsResultsFreezed = false;
            }
            else
            {
                Signatures = new SignaturesVM(statesAndSignatures.Signatures);
                IsObjectivesFreezed = statesAndSignatures.IsObjectivesFreezed;
                IsResultsFreezed = statesAndSignatures.IsResultsFreezed;
            }
        }
    }
}
