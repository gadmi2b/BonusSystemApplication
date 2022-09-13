using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient.Server;
using System.Globalization;
using System.Linq;

namespace BonusSystemApplication.Models.ViewModels.Index
{
    public class TableSelectLists
    {
        public GenericMultiSelectList<string, SelectEmployee> EmployeeSelectList { get; set; }

        public GenericMultiSelectList<Periods, SelectPeriod> PeriodSelectList { get; set; }

        public GenericMultiSelectList<int, SelectYear> YearSelectList { get; set; }

        public GenericMultiSelectList<Permissions, SelectPermission> PermissionSelectList { get; set; }

        public GenericMultiSelectList<string, SelectDepartment> DepartmentSelectList { get; set; }

        public GenericMultiSelectList<string, SelectTeam> TeamSelectList { get; set; }

        public GenericMultiSelectList<string, SelectWorkproject> WorkprojectSelectList { get; set; }

        public void PrepareMultiSelectLists(FormDataSorted formDataSorted, UserSelections userSelections)
        {
            EmployeeSelectList = new GenericMultiSelectList<string, SelectEmployee>(formDataSorted.SortedEmployees, userSelections.SelectedEmployees);
            PeriodSelectList = new GenericMultiSelectList<Periods, SelectPeriod>(formDataSorted.SortedPeriods, userSelections.SelectedPeriods);
            YearSelectList = new GenericMultiSelectList<int, SelectYear>(formDataSorted.SortedYears, userSelections.SelectedYears);
            PermissionSelectList = new GenericMultiSelectList<Permissions, SelectPermission>(formDataSorted.SortedPermissions, userSelections.SelectedPermissions);
            DepartmentSelectList = new GenericMultiSelectList<string, SelectDepartment>(formDataSorted.SortedDepartments, userSelections.SelectedDepartments);
            TeamSelectList = new GenericMultiSelectList<string, SelectTeam>(formDataSorted.SortedTeams, userSelections.SelectedTeams);
            WorkprojectSelectList = new GenericMultiSelectList<string, SelectWorkproject>(formDataSorted.SortedWorkprojects, userSelections.SelectedWorkprojects);
        }

    }
}
