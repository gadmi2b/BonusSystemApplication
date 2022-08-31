using Microsoft.AspNetCore.Mvc.Rendering;

namespace BonusSystemApplication.Models.ViewModels.Index
{
    public class TableFilters
    {
        public string Employee { get; set; }
        public GenericSelect<string> SelectEmployee { get; set; }

        public string Period { get; set; }
        public GenericSelect<Periods> SelectPeriod { get; set; }

        public long Year { get; set; }
        public GenericSelect<int> SelectYear { get; set; }

        public AccessFilter Access { get; set; }
        public GenericSelect<AccessFilter> SelectAccess { get; set; }

        public string Department { get; set; }
        public GenericSelect<string> SelectDepartment { get; set; }

        public string Team { get; set; }
        public GenericSelect<string> SelectTeam { get; set; }

        public string Workproject { get; set; }
        public GenericSelect<string> SelectWorkproject { get; set; }
    }
}
