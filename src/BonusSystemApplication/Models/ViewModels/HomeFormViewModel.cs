﻿using BonusSystemApplication.Models.ViewModels.FormViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BonusSystemApplication.Models.ViewModels
{
    public class HomeFormViewModel //Controller name | Action name | ViewModel
    {
        public ObjectivesDefinition ObjectivesDefinition;
        public ObjectivesSignature ObjectivesSignature;
        public ResultsDefinition ResultsDefinition;
        public ResultsSignature ResultsSignature;

        public List<SelectListItem> PeriodSelectList { get; set; }
        public List<SelectListItem> EmployeeSelectList { get; set; }
        public List<SelectListItem> WorkprojectSelectList { get; set; }
    }
}
