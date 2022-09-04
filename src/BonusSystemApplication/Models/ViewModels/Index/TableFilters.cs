using Microsoft.AspNetCore.Mvc.Rendering;

namespace BonusSystemApplication.Models.ViewModels.Index
{
    public class TableFilters
    {
        public List<string> Employees { get; set; }
        public GenericMultiSelectList<string, EmployeeSelect> EmployeeSelectList { get; set; }

        public List<string> Periods { get; set; }
        //public GenericMultiSelectList<Periods> SelectPeriod { get; set; }

        public List<string> Years { get; set; }
        //public GenericMultiSelectList<int> SelectYear { get; set; }

        public List<string> Access { get; set; }
        //public GenericMultiSelectList<AccessFilter> SelectAccess { get; set; }

        public List<string> Department { get; set; }
        //public GenericMultiSelectList<string> SelectDepartment { get; set; }

        public List<string> Team { get; set; }
        //public GenericMultiSelectList<string> SelectTeam { get; set; }

        public List<string> Workproject { get; set; }
        //public GenericMultiSelectList<string> SelectWorkproject { get; set; }

    }
}
