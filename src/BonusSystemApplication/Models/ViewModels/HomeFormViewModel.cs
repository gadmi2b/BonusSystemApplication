using Microsoft.AspNetCore.Mvc.Rendering;

namespace BonusSystemApplication.Models.ViewModels
{
    public class HomeFormViewModel //Controller name | Action name | ViewModel
    {
        public Definition Definition;
        public IList<ObjectiveResult> ObjectivesResults;
        public Conclusion Conclusion;
        public Signatures Signatures;

        public List<SelectListItem> PeriodSelectList { get; set; }
        public List<SelectListItem> EmployeeSelectList { get; set; }
        public List<SelectListItem> WorkprojectSelectList { get; set; }

        public long Id { get; set; }
        public string? TeamName { get; set; }
        public string? PositionName { get; set; }
        public string? Pid { get; set; }
        public string? WorkprojectDescription { get; set; }
        public bool IsObjectivesFreezed { get; set; }
        public bool IsResultsFreezed { get; set; }

    }
}
