using BonusSystemApplication.Models.ViewModels.FormViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BonusSystemApplication.Models.ViewModels
{
    public class HomeFormViewModel //Controller name | Action name | ViewModel
    {
        public Definition Definition;
        public ObjectivesSignature ObjectivesSignature;
        public Conclusion Conclusion;
        public ResultsSignature ResultsSignature;
        public IList<ObjectiveResult> ObjectivesResults;

        public List<SelectListItem> PeriodSelectList { get; set; }
        public List<SelectListItem> EmployeeSelectList { get; set; }
        public List<SelectListItem> WorkprojectSelectList { get; set; }
    }
}
