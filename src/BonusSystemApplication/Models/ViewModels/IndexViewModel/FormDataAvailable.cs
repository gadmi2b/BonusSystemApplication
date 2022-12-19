using BonusSystemApplication.Models.Repositories;
using System.Runtime.CompilerServices;

namespace BonusSystemApplication.Models.ViewModels.Index
{
    public class FormDataAvailable
    {
        public Dictionary<Form, List<Permission>> AvailableFormPermissions { get; }
        public List<Permission> AvailablePermissions { get; set; } = new List<Permission>();
        public List<string> AvailableEmployees { get; set; }
        public List<Periods> AvailablePeriods { get; set; }
        public List<int> AvailableYears { get; set; }
        public List<string> AvailableDepartments { get; set; }
        public List<string> AvailableTeams { get; set; }
        public List<string> AvailableWorkprojects { get; set; }

        public FormDataAvailable(Dictionary<Form, List<Permission>> availableFormPermissions)
        {
            AvailableFormPermissions = availableFormPermissions;

            List<Form> forms = availableFormPermissions.Keys.ToList();
            AvailableEmployees = FormDataExtractor.GetAvailableEmployees(forms);
            AvailablePeriods = FormDataExtractor.GetAvailablePeriods(forms);
            AvailableYears = FormDataExtractor.GetAvailableYears(forms);
            AvailableDepartments = FormDataExtractor.GetAvailableDepartments(forms);
            AvailableTeams = FormDataExtractor.GetAvailableTeams(forms);
            AvailableWorkprojects = FormDataExtractor.GetAvailableWorkprojects(forms);

            AvailablePermissions = FormDataExtractor.GetAvailablePermissions(availableFormPermissions.Values.ToList());
        }
    }
}
