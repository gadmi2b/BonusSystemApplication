using BonusSystemApplication.Models.Repositories;
using System.Runtime.CompilerServices;

namespace BonusSystemApplication.Models.ViewModels.Index
{
    public class FormDataAvailable
    {
        public List<Permissions> AvailablePermissions { get; set; } = new List<Permissions>();
        public List<string> AvailableEmployees { get; set; }
        public List<Periods> AvailablePeriods { get; set; }
        public List<int> AvailableYears { get; set; }
        public List<string> AvailableDepartments { get; set; }
        public List<string> AvailableTeams { get; set; }
        public List<string> AvailableWorkprojects { get; set; }

        public FormDataAvailable(Dictionary<Form, List<Permissions>> availableFormPermissions)
        {

            AvailablePermissions = GetAvailablePermissions(availableFormPermissions.Values.ToList());

            List<Form> forms = availableFormPermissions.Keys.ToList();
            AvailableEmployees = GetAvailableEmployees(forms);
            AvailablePeriods = GetAvailablePeriods(forms);
            AvailableYears = GetAvailableYears(forms);
            AvailableDepartments = GetAvailableDepartments(forms);
            AvailableTeams = GetAvailableTeams(forms);
            AvailableWorkprojects = GetAvailableWorkprojects(forms);
        }

        private List<Permissions> GetAvailablePermissions(List<List<Permissions>> allPermissions)
        {
            List<Permissions> permissions = new List<Permissions>();
            foreach (List<Permissions> list in allPermissions)
            {
                foreach (Permissions p in list)
                {
                    if (!permissions.Contains(p))
                    {
                        permissions.Add(p);
                    }
                }
            }
            return permissions;
        }
        private List<string> GetAvailableEmployees(List<Form> forms)
        {
            List<string> availableEmployees = new List<string>();
            availableEmployees = forms
                .Select(f => ($"{f.Definition.Employee.LastNameEng} {f.Definition.Employee.FirstNameEng}"))
                .Distinct()
                .ToList();

            return availableEmployees;
        }
        private List<Periods> GetAvailablePeriods(List<Form> forms)
        {
            List<Periods> availablePeriods = new List<Periods>();
            availablePeriods = forms
                .Select(f => f.Definition.Period)
                .Distinct()
                .ToList();

            return availablePeriods;
        }
        private List<int> GetAvailableYears(List<Form> forms)
        {
            List<int> availableYears = new List<int>();
            availableYears = forms
                .Select(f => f.Definition.Year)
                .Distinct()
                .ToList();

            return availableYears;
        }
        private List<string> GetAvailableDepartments(List<Form> forms)
        {
            List<string> availableDepartments = new List<string>();
            availableDepartments = forms
                .Select(f => f.Definition.Employee.Department.Name)
                .Distinct()
                .ToList();

            return availableDepartments;
        }
        private List<string> GetAvailableTeams(List<Form> forms)
        {
            List<string> availableTeams = new List<string>();
            availableTeams = forms
                .Select(f => f.Definition.Employee.Team.Name)
                .Distinct()
                .ToList();

            return availableTeams;
        }
        private List<string> GetAvailableWorkprojects(List<Form> forms)
        {
            List<string> availableWorkprojects = new List<string>();
            availableWorkprojects = forms
                .Select(f => f.Definition.Workproject.Name)
                .Distinct()
                .ToList();

            return availableWorkprojects;
        }
    }
}
