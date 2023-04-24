using BonusSystemApplication.DAL.Entities;

namespace BonusSystemApplication.BLL.Processes.Filtering
{
    /// <summary>
    /// contains all available forms data for current user
    /// </summary>
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
            AvailableEmployees = FormDataExtractor.ExtractEmployees(forms);
            AvailablePeriods = FormDataExtractor.ExtractPeriods(forms);
            AvailableYears = FormDataExtractor.ExtractYears(forms);
            AvailableDepartments = FormDataExtractor.ExtractDepartments(forms);
            AvailableTeams = FormDataExtractor.ExtractTeams(forms);
            AvailableWorkprojects = FormDataExtractor.ExtractWorkprojects(forms);

            AvailablePermissions = FormDataExtractor.ExtractPermissions(availableFormPermissions.Values.ToList());
        }
    }
}
