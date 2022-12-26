using BonusSystemApplication.Models.ViewModels.FormViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BonusSystemApplication.Models.ViewModels
{
    public class HomeFormViewModel //Controller name | Action name | ViewModel
    {
        public DefinitionViewModel Definition;
        public ConclusionViewModel Conclusion;
        public SignaturesViewModel Signatures;
        public IList<ObjectiveResultViewModel> ObjectivesResults;

        public List<SelectListItem> PeriodSelectList { get; set; }
        public List<SelectListItem> EmployeeSelectList { get; set; }
        public List<SelectListItem> WorkprojectSelectList { get; set; }

        public bool IsObjectivesFreezed { get; set; }
        public bool IsResultsFreezed { get; set; }

    }
}
