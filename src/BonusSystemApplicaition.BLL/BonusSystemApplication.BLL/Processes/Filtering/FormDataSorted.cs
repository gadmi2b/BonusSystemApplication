using BonusSystemApplication.BLL.DTO.Index;
using BonusSystemApplication.DAL.Entities;

namespace BonusSystemApplication.BLL.Processes.Filtering
{
    public class FormDataSorted
    {
        public Dictionary<Form, List<Permission>> SortedFormPermissions { get; } = new Dictionary<Form, List<Permission>>();
        public List<Permission> SortedPermissions { get; set; }
        public List<string> SortedEmployees { get; set; }
        public List<Periods> SortedPeriods { get; set; }
        public List<int> SortedYears { get; set; }
        public List<string> SortedDepartments { get; set; }
        public List<string> SortedTeams { get; set; }
        public List<string> SortedWorkprojects { get; set; }

        public FormDataSorted(FormDataAvailable formDataAvailable, UserSelectionsDTO userSelections)
        {
            foreach (Form form in formDataAvailable.AvailableFormPermissions.Keys)
            {
                List<Permission> permissions = formDataAvailable.AvailableFormPermissions[form];
                if (IsFormCanBeShown(form, userSelections, permissions))
                {
                    SortedFormPermissions.Add(form, permissions);
                }
            }

            List<Form> sortedForms = SortedFormPermissions.Keys.ToList();
            SortedEmployees = FormDataExtractor.ExtractEmployees(sortedForms);
            SortedPeriods = FormDataExtractor.ExtractPeriods(sortedForms);
            SortedYears = FormDataExtractor.ExtractYears(sortedForms);
            SortedDepartments = FormDataExtractor.ExtractDepartments(sortedForms);
            SortedTeams = FormDataExtractor.ExtractTeams(sortedForms);
            SortedWorkprojects = FormDataExtractor.ExtractWorkprojects(sortedForms);

            SortedPermissions = FormDataExtractor.ExtractPermissions(SortedFormPermissions.Values.ToList());
        }

        private bool IsFormCanBeShown(Form form, UserSelectionsDTO userSelections, List<Permission> formPermissions)
        {
            if (IsValueSelected(userSelections.SelectedEmployees, $"{form.Definition.Employee.LastNameEng} {form.Definition.Employee.FirstNameEng}") &&
                IsValueSelected(userSelections.SelectedPeriods, form.Definition.Period.ToString()) &&
                IsValueSelected(userSelections.SelectedYears, form.Definition.Year.ToString()) &&
                IsValueSelected(userSelections.SelectedDepartments, form.Definition.Employee.Department.Name) &&
                IsValueSelected(userSelections.SelectedTeams, form.Definition.Employee.Team.Name) &&
                IsValueSelected(userSelections.SelectedWorkprojects, form.Definition.Workproject.Name) &&
                IsValueSelected(userSelections.SelectedPermissions, formPermissions.Select(p => p.ToString()).ToList()))
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
        private bool IsValueSelected(List<string> selectedCollection, List<string> checkValues)
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
