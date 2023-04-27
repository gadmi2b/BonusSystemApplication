﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        public IList<ObjectiveResultVM> ObjectivesResults { get; set; }

        [ValidateNever]
        public List<SelectListItem> PeriodSelectList { get; set; }
        [ValidateNever]
        public List<SelectListItem> EmployeeSelectList { get; set; }
        [ValidateNever]
        public List<SelectListItem> WorkprojectSelectList { get; set; }
    }
}
