using BonusSystemApplication.DAL.Entities;
using BonusSystemApplication.Models.ViewModels.FormViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BonusSystemApplication.Models.ViewModels
{
    public class HomeFormViewModel //Controller name | Action name | ViewModel
    {
        public DefinitionViewModel Definition { get; set; }
        public ConclusionViewModel Conclusion { get; set; }
        public SignaturesViewModel Signatures { get; set; }
        public IList<ObjectiveResultViewModel> ObjectivesResults { get; set; }

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
            Definition = new DefinitionViewModel();
            Conclusion = new ConclusionViewModel();
            Signatures = new SignaturesViewModel
            {
                ForObjectives = new ForObjectives(),
                ForResults = new ForResults(),
            };

            List<ObjectiveResultViewModel> objectivesResults = new List<ObjectiveResultViewModel>();
            for (int i = 0; i < 10; i++)
            {
                ObjectiveResultViewModel objectiveResult = new ObjectiveResultViewModel()
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

            Definition = new DefinitionViewModel(form.Definition);
            Conclusion = new ConclusionViewModel(form.Conclusion);
            Signatures = new SignaturesViewModel(form.Signatures);

            foreach (var objRes in form.ObjectivesResults)
            {
                ObjectiveResultViewModel objResViewModel = new ObjectiveResultViewModel(objRes);
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
                Signatures = new SignaturesViewModel
                {
                    ForObjectives = new ForObjectives(),
                    ForResults = new ForResults(),
                };
                IsObjectivesFreezed = false;
                IsResultsFreezed = false;
            }
            else
            {
                Signatures = new SignaturesViewModel(statesAndSignatures.Signatures);
                IsObjectivesFreezed = statesAndSignatures.IsObjectivesFreezed;
                IsResultsFreezed = statesAndSignatures.IsResultsFreezed;
            }
        }
    }
}
