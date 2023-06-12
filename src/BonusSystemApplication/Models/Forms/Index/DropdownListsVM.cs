using Microsoft.AspNetCore.Mvc.Rendering;

namespace BonusSystemApplication.Models.Forms.Index
{
    public class DropdownListsVM
    {
        public List<SelectListItem> EmployeeDropdownList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> PeriodDropdownList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> YearDropdownList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> PermissionDropdownList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> DepartmentDropdownList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> TeamDropdownList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> WorkprojectDropdownList { get; set; } = new List<SelectListItem>();
    }
}
