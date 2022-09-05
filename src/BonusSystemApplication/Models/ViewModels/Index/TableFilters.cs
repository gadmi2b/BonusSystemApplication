using Microsoft.AspNetCore.Mvc.Rendering;

namespace BonusSystemApplication.Models.ViewModels.Index
{
    public class TableFilters
    {
        public List<string> Employees { get; set; }
        public GenericMultiSelectList<string, SelectEmployee> EmployeeSelectList { get; set; }

        public List<string> Periods { get; set; }
        public GenericMultiSelectList<Periods, SelectPeriod> PeriodSelectList { get; set; }

        public List<string> Years { get; set; }
        public GenericMultiSelectList<int, SelectYear> YearSelectList { get; set; }

        public List<string> Permissions { get; set; }
        public GenericMultiSelectList<Permissions, SelectPermission> PermissionSelectList { get; set; }

        public List<string> Departments { get; set; }
        public GenericMultiSelectList<string, SelectDepartment> DepartmentSelectList { get; set; }

        public List<string> Teams { get; set; }
        public GenericMultiSelectList<string, SelectTeam> TeamSelectList { get; set; }

        public List<string> Workprojects { get; set; }
        public GenericMultiSelectList<string, SelectWorkproject> WorkprojectSelectList { get; set; }
    }

    public void PrepareMultiSelectLists(FormData formData)
    {
        EmployeeSelectList = new GenericMultiSelectList<string, SelectEmployee>(formData.AvailableEmployees, null);
        PeriodSelectList = new GenericMultiSelectList<Periods, SelectPeriod>(formData.AvailablePeriods, null);
        YearSelectList = new GenericMultiSelectList<int, SelectYear>(formData.AvailableYears, null);
        PermissionSelectList = new GenericMultiSelectList<Permissions, SelectPermission>(formData.AvailablePermissions, null);
        DepartmentSelectList = new GenericMultiSelectList<string, SelectDepartment>(formData.AvailableDepartments, null);
        TeamSelectList = new GenericMultiSelectList<string, SelectTeam>(formData.AvailableTeams, null);
        WorkprojectSelectList = new GenericMultiSelectList<string, SelectWorkproject>(formData.AvailableWorkprojects, null);
    }
}
