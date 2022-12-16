using BonusSystemApplication.Models.Repositories;
using BonusSystemApplication.UserIdentiry;

namespace BonusSystemApplication.Models.ViewModels.Index
{
    public class FormDataSorted
    {
        public Dictionary<Form, List<string>> FormAndPermissions { get; } = new Dictionary<Form, List<string>>();

        public List<Permissions> SortedPermissions { get; set; }
        public List<string> SortedEmployees { get; set; }
        public List<Periods> SortedPeriods { get; set; }
        public List<int> SortedYears { get; set; }
        public List<string> SortedDepartments { get; set; }
        public List<string> SortedTeams { get; set; }
        public List<string> SortedWorkprojects { get; set; }

        public FormDataSorted(List<Form> availableForms, long userId, IEnumerable<GlobalAccess> globalAccesses, UserSelections userSelections)
        {
            List<string> permissionNames = new List<string>();
            foreach(Form f in availableForms)
            {
                if(IsFormHasPermissions(f, globalAccesses, out permissionNames) &&
                   IsFormCanBeShown(f, permissionNames, userSelections))
                {
                    FormAndPermissions.Add(f, permissionNames);
                }
            }

            IFormDataExtractor formDataExtractor = new FormDataExtractor();
            List<Form> sortedForms = FormAndPermissions.Keys.ToList();

            SortedPermissions = formDataExtractor.GetAvailablePermissions(sortedForms, userId, globalAccesses);
            SortedEmployees = formDataExtractor.GetAvailableEmployees(sortedForms);
            SortedPeriods = formDataExtractor.GetAvailablePeriods(sortedForms);
            SortedYears = formDataExtractor.GetAvailableYears(sortedForms);
            SortedDepartments = formDataExtractor.GetAvailableDepartments(sortedForms);
            SortedTeams = formDataExtractor.GetAvailableTeams(sortedForms);
            SortedWorkprojects = formDataExtractor.GetAvailableWorkprojects(sortedForms);
        }
        private bool IsFormHasPermissions(Form form, IEnumerable<GlobalAccess> globalAccesses, out List<string> permissionNames)
        {
            long userId = UserData.UserId;

            List<Permissions> permissionsTemp = new List<Permissions>();
            Func<Form, bool> delegatePermission;

            foreach (var formGA in globalAccesses)
            {
                delegatePermission = ExpressionBuilder.GetExpressionForGlobalAccess(formGA).Compile();
                if (delegatePermission.Invoke(form))
                {
                    permissionsTemp.Add(Permissions.GlobalAccess);
                }
            }

            delegatePermission = ExpressionBuilder.GetExpressionForLocalAccess(userId).Compile();
            if (delegatePermission.Invoke(form))
            {
                permissionsTemp.Add(Permissions.LocalAccess);
            }

            delegatePermission = ExpressionBuilder.GetMethodForParticipation(userId, Permissions.Employee);
            if (delegatePermission.Invoke(form))
            {
                permissionsTemp.Add(Permissions.Employee);
            }

            delegatePermission = ExpressionBuilder.GetMethodForParticipation(userId, Permissions.Manager);
            if (delegatePermission.Invoke(form))
            {
                permissionsTemp.Add(Permissions.Manager);
            }

            delegatePermission = ExpressionBuilder.GetMethodForParticipation(userId, Permissions.Approver);
            if (delegatePermission.Invoke(form))
            {
                permissionsTemp.Add(Permissions.Approver);
            }

            List<string> permissionNamesTemp = new List<string>();
            foreach (Permissions p in permissionsTemp)
            {
                permissionNamesTemp.Add(p.ToString());
            }
            permissionNames = permissionNamesTemp;

            if (permissionNamesTemp.Count > 0) return true;
            else return false;
        }
        private bool IsFormCanBeShown(Form form, List<string> formPermissions, UserSelections userSelections)
        {
            if (isValueSelected(userSelections.SelectedEmployees, $"{form.Definition.Employee.LastNameEng} {form.Definition.Employee.FirstNameEng}") &&
                isValueSelected(userSelections.SelectedPeriods, form.Definition.Period.ToString()) &&
                isValueSelected(userSelections.SelectedYears, form.Definition.Year.ToString()) &&
                isValueSelected(userSelections.SelectedDepartments, form.Definition.Employee.Department.Name) &&
                isValueSelected(userSelections.SelectedTeams, form.Definition.Employee.Team.Name) &&
                isValueSelected(userSelections.SelectedWorkprojects, form.Definition.Workproject.Name) &&
                isValueSelected(userSelections.SelectedPermissions, formPermissions))
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
