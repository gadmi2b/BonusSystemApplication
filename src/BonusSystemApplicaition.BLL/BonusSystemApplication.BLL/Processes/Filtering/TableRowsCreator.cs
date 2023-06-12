using BonusSystemApplication.BLL.DTO.Index;
using BonusSystemApplication.DAL.Entities;

namespace BonusSystemApplication.BLL.Processes.Filtering
{
    public class TableRowsCreator
    {
        private FormDataAvailable _formDataAvailable { get; set; }
        private UserSelectionsDTO _userSelections { get; set; }

        public TableRowsCreator(FormDataAvailable formDataAvailable, UserSelectionsDTO userSelections)
        {
            _formDataAvailable = formDataAvailable;
            _userSelections = userSelections;
        }

        public List<TableRowDTO> CreateTableRows()
        {
            Dictionary<Form, List<Permission>> formPermissions = GetSortedFormPermissions();
            return formPermissions
                    .Select(pair => new TableRowDTO
                    {
                        Id = pair.Key.Id,
                        EmployeeFullName = $"{pair.Key.Definition.Employee.LastNameEng} {pair.Key.Definition.Employee.FirstNameEng}",
                        WorkprojectName = pair.Key.Definition.Workproject.Name,
                        DepartmentName = pair.Key.Definition.Employee.Department?.Name == null ? string.Empty : pair.Key.Definition.Employee.Department.Name,
                        TeamName = pair.Key.Definition.Employee.Team?.Name == null ? string.Empty : pair.Key.Definition.Employee.Team.Name,
                        LastSavedAt = pair.Key.LastSavedAt,
                        Period = pair.Key.Definition.Period.ToString(),
                        Year = pair.Key.Definition.Year.ToString(),
                        Permissions = pair.Value.Select(p => p.ToString()).ToList(),
                    })
                    .ToList();
        }

        private Dictionary<Form, List<Permission>> GetSortedFormPermissions()
        {
            Dictionary<Form, List<Permission>> formPermissions = new Dictionary<Form, List<Permission>>();
            foreach (Form form in _formDataAvailable.AvailableFormPermissions.Keys)
            {
                List<Permission> permissions = _formDataAvailable.AvailableFormPermissions[form];
                if (IsFormCanBeShown(form, permissions))
                {
                    formPermissions.Add(form, permissions);
                }
            }

            return formPermissions;
        }
        private bool IsFormCanBeShown(Form form, List<Permission> formPermissions)
        {
            if (IsValueSelected(_userSelections.SelectedEmployees, $"{form.Definition.Employee.LastNameEng} {form.Definition.Employee.FirstNameEng}") &&
                IsValueSelected(_userSelections.SelectedPeriods, form.Definition.Period.ToString()) &&
                IsValueSelected(_userSelections.SelectedYears, form.Definition.Year.ToString()) &&
                IsValueSelected(_userSelections.SelectedDepartments, form.Definition.Employee.Department?.Name) &&
                IsValueSelected(_userSelections.SelectedTeams, form.Definition.Employee.Team?.Name) &&
                IsValueSelected(_userSelections.SelectedWorkprojects, form.Definition.Workproject.Name) &&
                IsValueSelected(_userSelections.SelectedPermissions, formPermissions.Select(p => p.ToString()).ToList()))
            {
                return true;
            }

            return false;
        }
        private bool IsValueSelected(List<string> selectedCollection, string checkValue)
        {
            if (selectedCollection.Count > 1)   // every selectedCollection has empty string at 0 position
            {
                if (selectedCollection.Contains(checkValue))
                    return true;
            }
            else
                return true;

            return false;
        }
        private bool IsValueSelected(List<string> selectedCollection, List<string> checkValues)
        {
            if (selectedCollection.Count > 1)   // every selectedCollection has empty string at 0 position
            {
                foreach (string s in checkValues)
                {
                    if (selectedCollection.Contains(s))
                        return true;
                }
            }
            else
                return true;

            return false;
        }
    }
}
