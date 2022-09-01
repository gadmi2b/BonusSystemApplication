using Microsoft.AspNetCore.Mvc.Rendering;

namespace BonusSystemApplication.Models.ViewModels.Index
{
    public class TableFilters
    {
        public string Employee { get; set; } = string.Empty;
        public GenericSelect<string> SelectEmployee { get; set; }

        public string Period { get; set; } = string.Empty;
        public GenericSelect<Periods> SelectPeriod { get; set; }

        public string Year { get; set; } = string.Empty;
        public GenericSelect<int> SelectYear { get; set; }

        public string Access { get; set; } = string.Empty;
        public GenericSelect<AccessFilter> SelectAccess { get; set; }

        public string Department { get; set; } = string.Empty;
        public GenericSelect<string> SelectDepartment { get; set; }

        public string Team { get; set; } = string.Empty;
        public GenericSelect<string> SelectTeam { get; set; }

        public string Workproject { get; set; } = string.Empty;
        public GenericSelect<string> SelectWorkproject { get; set; }
    }
}
