using BonusSystemApplication.Models.Repositories;
using System.Runtime.CompilerServices;

namespace BonusSystemApplication.Models.ViewModels.Index
{
    public class FormDataAvailable
    {
        public List<Permissions> AvailablePermissions { get; set; }
        public List<string> AvailableEmployees { get; set; }
        public List<Periods> AvailablePeriods { get; set; }
        public List<int> AvailableYears { get; set; }
        public List<string> AvailableDepartments { get; set; }
        public List<string> AvailableTeams { get; set; }
        public List<string> AvailableWorkprojects { get; set; }

        public FormDataAvailable(List<Form> availableForms, long userId, IEnumerable<GlobalAccess> globalAccesses)
        {
            IFormDataExtractor formDataExtractor = new FormDataExtractor();

            AvailablePermissions = formDataExtractor.GetAvailablePermissions(availableForms, userId, globalAccesses);
            AvailableEmployees = formDataExtractor.GetAvailableEmployees(availableForms);
            AvailablePeriods = formDataExtractor.GetAvailablePeriods(availableForms);
            AvailableYears = formDataExtractor.GetAvailableYears(availableForms);
            AvailableDepartments = formDataExtractor.GetAvailableDepartments(availableForms);
            AvailableTeams = formDataExtractor.GetAvailableTeams(availableForms);
            AvailableWorkprojects = formDataExtractor.GetAvailableWorkprojects(availableForms);
        }
    }
}
