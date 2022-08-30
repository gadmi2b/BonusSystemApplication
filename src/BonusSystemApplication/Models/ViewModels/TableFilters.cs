using Microsoft.AspNetCore.Mvc.Rendering;

namespace BonusSystemApplication.Models.ViewModels
{
    public class TableFilters
    {
        public string Employee { get; set; }
        public List<SelectListItem> SelectEmployees { get; set; } = new List<SelectListItem>();

        public string Period { get; set; }
        public List<SelectListItem> SelectPeriods { get; set; } = new List<SelectListItem>();

        public long Year { get; set; }
        public List<SelectListItem> SelectYears { get; set; } = new List<SelectListItem>();

        public AccessFilter Access { get; set; }
        public List<SelectListItem> SelectAccesses { get; set; } = new List<SelectListItem>();

        public string Department { get; set; }
        public List<SelectListItem> SelectDepartments { get; set; } = new List<SelectListItem>();

        public string Team { get; set; }
        public List<SelectListItem> SelectTeams { get; set; } = new List<SelectListItem>();

        public string Workproject { get; set; }
        public List<SelectListItem> SelectWorkprojects { get; set; } = new List<SelectListItem>();

        public TableFilters() { }
    }
}

//public List<SelectListItem> SelectEmployees { get; set; } = new List<SelectListItem>
//{
//    new SelectListItem { Value = "1", Text = "Ivan Poddubniy" },
//    new SelectListItem { Value = "2", Text = "Petr Traktorov" },
//    new SelectListItem { Value = "3", Text = "Alex Toporov"  },
//};
