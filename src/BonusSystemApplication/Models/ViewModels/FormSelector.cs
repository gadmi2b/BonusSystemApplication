using Microsoft.AspNetCore.Mvc.Rendering;

namespace BonusSystemApplication.Models.ViewModels
{
    public class FormSelector
    {
        public SelectListItem SelectYears { get; set; }
        public SelectListItem SelectPriods { get; set; }
        public SelectListItem SelectAccessFilters { get; set; }
        public SelectListItem SelectDepartments { get; set; }
        public SelectListItem SelectTeams { get; set; }
        public SelectListItem SelectWorkprojects { get; set; }


        public string Employees { get; set; }

        public List<SelectListItem> SelectEmployees { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Ivan Poddubniy" },
            new SelectListItem { Value = "2", Text = "Petr Traktorov" },
            new SelectListItem { Value = "3", Text = "Alex Toporov"  },
        };
    }
}
