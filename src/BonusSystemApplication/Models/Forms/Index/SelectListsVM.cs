using Microsoft.AspNetCore.Mvc.Rendering;

namespace BonusSystemApplication.Models.Forms.Index
{
    public class SelectListsVM
    {
        public List<SelectListItem> EmployeeSelectListItems { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> PeriodSelectListItems { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> YearSelectListItems { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> PermissionSelectListItems { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> DepartmentSelectListItems { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> TeamSelectListItems { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> WorkprojectSelectListItems { get; set; } = new List<SelectListItem>();
    }
}
