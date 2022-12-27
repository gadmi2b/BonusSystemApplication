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
    }
}
