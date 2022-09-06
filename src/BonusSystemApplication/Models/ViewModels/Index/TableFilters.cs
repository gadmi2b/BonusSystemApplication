using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient.Server;
using System.Globalization;
using System.Linq;

namespace BonusSystemApplication.Models.ViewModels.Index
{
    public class TableFilters
    {
        public List<string> SelectedEmployees { get; set; } = new List<string>();
        public GenericMultiSelectList<string, SelectEmployee> EmployeeSelectList { get; set; }

        public List<string> SelectedPeriods { get; set; } = new List<string>();
        public GenericMultiSelectList<Periods, SelectPeriod> PeriodSelectList { get; set; }

        public List<string> SelectedYears { get; set; } = new List<string>();
        public GenericMultiSelectList<int, SelectYear> YearSelectList { get; set; }

        public List<string> SelectedPermissions { get; set; } = new List<string>();
        public GenericMultiSelectList<Permissions, SelectPermission> PermissionSelectList { get; set; }

        public List<string> SelectedDepartments { get; set; } = new List<string>();
        public GenericMultiSelectList<string, SelectDepartment> DepartmentSelectList { get; set; }

        public List<string> SelectedTeams { get; set; } = new List<string>();
        public GenericMultiSelectList<string, SelectTeam> TeamSelectList { get; set; }

        public List<string> SelectedWorkprojects { get; set; } = new List<string>();
        public GenericMultiSelectList<string, SelectWorkproject> WorkprojectSelectList { get; set; }

        public void PrepareMultiSelectLists(FormDataSingleton formData)
        {
            RemoveDistinct();
            ValidateSelections(formData);

            EmployeeSelectList = new GenericMultiSelectList<string, SelectEmployee>(formData.AvailableEmployees, SelectedEmployees);
            PeriodSelectList = new GenericMultiSelectList<Periods, SelectPeriod>(formData.AvailablePeriods, SelectedPeriods);
            YearSelectList = new GenericMultiSelectList<int, SelectYear>(formData.AvailableYears, SelectedYears);
            PermissionSelectList = new GenericMultiSelectList<Permissions, SelectPermission>(formData.AvailablePermissions, SelectedPermissions);
            DepartmentSelectList = new GenericMultiSelectList<string, SelectDepartment>(formData.AvailableDepartments, SelectedDepartments);
            TeamSelectList = new GenericMultiSelectList<string, SelectTeam>(formData.AvailableTeams, SelectedTeams);
            WorkprojectSelectList = new GenericMultiSelectList<string, SelectWorkproject>(formData.AvailableWorkprojects, SelectedWorkprojects);
        }
        private void ValidateSelections(FormDataSingleton formData)
        {
            List<string> itemsToRemove = new List<string>();
            //Selected Employees validation
            PrepareSelections(SelectedEmployees);
            foreach (string item in SelectedEmployees)
            {
                if (string.IsNullOrEmpty(item)
                    ? false
                    : !formData.AvailableEmployees.Contains(item))
                {
                    //SelectedEmployees.Remove(item);
                    itemsToRemove.Add(item);
                }
            }
            SelectedEmployees.RemoveAll(x => itemsToRemove.Contains(x));
            itemsToRemove.Clear();

            //Selected Periods validation
            PrepareSelections(SelectedPeriods);
            foreach (string item in SelectedPeriods)
            {
                if (string.IsNullOrEmpty(item)
                    ? false
                    : (Enum.TryParse(item, out Periods result) && !formData.AvailablePeriods.Contains(result)) ||
                      !Enum.TryParse(item, out result))
                {
                    //SelectedPeriods.Remove(item);
                    itemsToRemove.Add(item);
                }
            }
            SelectedPeriods.RemoveAll(x => itemsToRemove.Contains(x));
            itemsToRemove.Clear();

            //Selected Years validation
            PrepareSelections(SelectedYears);
            foreach (string item in SelectedYears)
            {
                if (string.IsNullOrEmpty(item)
                    ? false
                    : (Int32.TryParse(item, out int result) && !formData.AvailableYears.Contains(result)) ||
                      !Int32.TryParse(item, out result))
                {
                    itemsToRemove.Add(item);
                }
            }
            SelectedYears.RemoveAll(x => itemsToRemove.Contains(x));
            itemsToRemove.Clear();

            //Selected Permissions validation
            PrepareSelections(SelectedPermissions);
            foreach (string item in SelectedPermissions)
            {
                if (string.IsNullOrEmpty(item)
                    ? false
                    : (Enum.TryParse(item, out Permissions result) && !formData.AvailablePermissions.Contains(result)) ||
                      !Enum.TryParse(item, out result))
                {
                    itemsToRemove.Add(item);
                }
            }
            SelectedPermissions.RemoveAll(x => itemsToRemove.Contains(x));
            itemsToRemove.Clear();

            //Selected Departments validation
            PrepareSelections(SelectedDepartments);
            foreach (string item in SelectedDepartments)
            {
                if (string.IsNullOrEmpty(item)
                    ? false
                    : !formData.AvailableDepartments.Contains(item))
                {
                    itemsToRemove.Add(item);
                }
            }
            SelectedDepartments.RemoveAll(x => itemsToRemove.Contains(x));
            itemsToRemove.Clear();

            //Selected Teams validation
            PrepareSelections(SelectedTeams);
            foreach (string item in SelectedTeams)
            {
                if (string.IsNullOrEmpty(item)
                    ? false
                    : !formData.AvailableTeams.Contains(item))
                {
                    itemsToRemove.Add(item);
                }
            }
            SelectedTeams.RemoveAll(x => itemsToRemove.Contains(x));
            itemsToRemove.Clear();

            //Selected Workprojects validation
            PrepareSelections(SelectedWorkprojects);
            foreach (string item in SelectedWorkprojects)
            {
                if (string.IsNullOrEmpty(item)
                    ? false
                    : !formData.AvailableWorkprojects.Contains(item))
                {
                    itemsToRemove.Add(item);
                }
            }
            SelectedWorkprojects.RemoveAll(x => itemsToRemove.Contains(x));
            itemsToRemove.Clear();

        }
        private void PrepareSelections(List<string> SelectedCollection)
        {
            SelectedCollection.RemoveAll(x => x == string.Empty);
            SelectedCollection.OrderByDescending(x => x);
            SelectedCollection.Insert(0, string.Empty);
        }
        private void RemoveDistinct()
        {
            SelectedEmployees = SelectedEmployees.Distinct().ToList();
            SelectedPeriods = SelectedPeriods.Distinct().ToList();
            SelectedYears = SelectedYears.Distinct().ToList();
            SelectedPermissions = SelectedPermissions.Distinct().ToList();
            SelectedDepartments = SelectedDepartments.Distinct().ToList();
            SelectedTeams = SelectedTeams.Distinct().ToList();
            SelectedWorkprojects = SelectedWorkprojects.Distinct().ToList();
        }

        public bool IsFormCanBeShown(Form f, List<string> formPermissions)
        {
            if(isValueSelected(SelectedEmployees, $"{f.Employee.LastNameEng} {f.Employee.FirstNameEng}") &&
               isValueSelected(SelectedPeriods, f.Period.ToString()) &&
               isValueSelected(SelectedYears, f.Year.ToString()) &&
               isValueSelected(SelectedDepartments, f.Employee.Department.Name) &&
               isValueSelected(SelectedTeams, f.Employee.Team.Name) &&
               isValueSelected(SelectedWorkprojects, f.Workproject.Name) &&
               isValueSelected(SelectedPermissions, formPermissions))
            {
                return true;
            }

            return false;
        }
        private bool isValueSelected(List<string> selectedCollection, string checkValue)
        {
            if (selectedCollection.Count > 1)   // every selectedCollection has empty string at 0 position
            {
                if (selectedCollection.Contains(checkValue))
                { 
                    return true; 
                }
            }
            else
            {
                return true;
            }

            return false;
        }
        private bool isValueSelected(List<string> selectedCollection, List<string> checkValues)
        {
            if (selectedCollection.Count > 1)   // every selectedCollection has empty string at 0 position
            {
                foreach (string s in checkValues)
                {
                    if (selectedCollection.Contains(s))
                    {
                        return true;
                    }
                }
            }
            else
            {
                return true;
            }

            return false;
        }
    }
}
