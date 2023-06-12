using BonusSystemApplication.DAL.Entities;

namespace BonusSystemApplication.BLL.Processes.Filtering
{
    /// <summary>
    /// contains all available forms data for current user
    /// </summary>
    public class FormDataAvailable
    {
        FormDataExtractor _formDataExtractor { get; set; }

        public Dictionary<Form, List<Permission>> AvailableFormPermissions { get; set; }
        public List<Permission> AvailablePermissions { get; set; } = new List<Permission>();
        public List<string> AvailableEmployees { get; set; }
        public List<Periods> AvailablePeriods { get; set; }
        public List<int> AvailableYears { get; set; }
        public List<string> AvailableDepartments { get; set; }
        public List<string> AvailableTeams { get; set; }
        public List<string> AvailableWorkprojects { get; set; }

        public FormDataAvailable(FormDataExtractor formDataExtractor)
        {
            _formDataExtractor = formDataExtractor;
        }

        public void PrepareAvailableFormPermissions(long userId,
                                                    List<Form> forms,
                                                    IEnumerable<long> formIdsWithGlobalAccess,
                                                    IEnumerable<long> formIdsWithLocalAccess,
                                                    IEnumerable<long> formIdsWithParticipation)
        {
            AvailableFormPermissions = forms
                          .ToDictionary(form => form,
                                        form => _formDataExtractor.ExtractPermissions(form,
                                                                                    userId,
                                                                                    formIdsWithGlobalAccess,
                                                                                    formIdsWithLocalAccess,
                                                                                    formIdsWithParticipation));
        }
        public void PrepareAvailablePermissions(List<Form> forms)
        {
            AvailablePermissions = _formDataExtractor.ExtractPermissions(AvailableFormPermissions.Values.ToList());
        }
        public void PrepareAvailableEmployees(List<Form> forms)
        {
            AvailableEmployees = _formDataExtractor.ExtractEmployees(forms);
        }
        public void PrepareAvailablePeriods(List<Form> forms)
        {
            AvailablePeriods = _formDataExtractor.ExtractPeriods(forms);
        }
        public void PrepareAvailableYears(List<Form> forms)
        {
            AvailableYears = _formDataExtractor.ExtractYears(forms);
        }
        public void PrepareAvailableDepartments(List<Form> forms)
        {
            AvailableDepartments = _formDataExtractor.ExtractDepartments(forms);
        }
        public void PrepareAvailableTeams(List<Form> forms)
        {
            AvailableTeams = _formDataExtractor.ExtractTeams(forms);
        }
        public void PrepareAvailableWorkprojects(List<Form> forms)
        {
            AvailableWorkprojects = _formDataExtractor.ExtractWorkprojects(forms);
        }
    }
}
