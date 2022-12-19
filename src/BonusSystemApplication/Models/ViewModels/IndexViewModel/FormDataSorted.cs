using BonusSystemApplication.Models.Repositories;
using BonusSystemApplication.UserIdentiry;

namespace BonusSystemApplication.Models.ViewModels.Index
{
    public class FormDataSorted
    {
        public Dictionary<Form, List<Permissions>> SortedFormPermissions { get; } = new Dictionary<Form, List<Permissions>>();
        public List<Permissions> SortedPermissions { get; set; }
        public List<string> SortedEmployees { get; set; }
        public List<Periods> SortedPeriods { get; set; }
        public List<int> SortedYears { get; set; }
        public List<string> SortedDepartments { get; set; }
        public List<string> SortedTeams { get; set; }
        public List<string> SortedWorkprojects { get; set; }

        public FormDataSorted(FormDataAvailable formDataAvailable, UserSelections userSelections)
        {
            foreach(Form f in formDataAvailable.AvailableFormPermissions.Keys)
            {
                List<Permissions> permissions = formDataAvailable.AvailableFormPermissions[f];
                if (IsFormCanBeShown(f, userSelections, permissions))
                {
                    SortedFormPermissions.Add(f, permissions);
                }
            }

            List<Form> sortedForms = SortedFormPermissions.Keys.ToList();
            SortedEmployees = FormDataExtractor.GetAvailableEmployees(sortedForms);
            SortedPeriods = FormDataExtractor.GetAvailablePeriods(sortedForms);
            SortedYears = FormDataExtractor.GetAvailableYears(sortedForms);
            SortedDepartments = FormDataExtractor.GetAvailableDepartments(sortedForms);
            SortedTeams = FormDataExtractor.GetAvailableTeams(sortedForms);
            SortedWorkprojects = FormDataExtractor.GetAvailableWorkprojects(sortedForms);

            SortedPermissions = FormDataExtractor.GetAvailablePermissions(SortedFormPermissions.Values.ToList());
        }

        private bool IsFormCanBeShown(Form form, UserSelections userSelections, List<Permissions> formPermissions)
        {
            if (isValueSelected(userSelections.SelectedEmployees, $"{form.Definition.Employee.LastNameEng} {form.Definition.Employee.FirstNameEng}") &&
                isValueSelected(userSelections.SelectedPeriods, form.Definition.Period.ToString()) &&
                isValueSelected(userSelections.SelectedYears, form.Definition.Year.ToString()) &&
                isValueSelected(userSelections.SelectedDepartments, form.Definition.Employee.Department.Name) &&
                isValueSelected(userSelections.SelectedTeams, form.Definition.Employee.Team.Name) &&
                isValueSelected(userSelections.SelectedWorkprojects, form.Definition.Workproject.Name) &&
                isValueSelected(userSelections.SelectedPermissions, formPermissions.Select(p => p.ToString()).ToList()))
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
