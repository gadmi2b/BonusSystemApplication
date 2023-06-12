using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BonusSystemApplication.Models.Forms.Edit
{
    public class FormEditViewModel
    {
        public long Id { get; set; }
        public bool AreObjectivesFrozen { get; set; }
        public bool AreResultsFrozen { get; set; }

        public DefinitionVM Definition { get; set; }
        public ConclusionVM Conclusion { get; set; }
        public SignaturesVM Signatures { get; set; }
        public List<ObjectiveResultVM> ObjectivesResults { get; set; }

        [ValidateNever]
        public List<SelectListItem> WorkprojectsSelectList { get; set; }
        [ValidateNever]
        public List<SelectListItem> EmployeesSelectList { get; set; }
        [ValidateNever]
        public List<SelectListItem> ManagersSelectList { get; set; }
        [ValidateNever]
        public List<SelectListItem> ApproversSelectList { get; set; }
        [ValidateNever]
        public List<SelectListItem> PeriodsSelectList { get; set; }
    }
}
