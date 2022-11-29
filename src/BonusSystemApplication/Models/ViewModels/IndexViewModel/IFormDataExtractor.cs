namespace BonusSystemApplication.Models.ViewModels.Index
{
    public interface IFormDataExtractor
    {
        public List<string> GetAvailableEmployees(List<Form> forms);
        public List<Periods> GetAvailablePeriods(List<Form> forms);
        public List<int> GetAvailableYears(List<Form> forms);
        public List<string> GetAvailableDepartments(List<Form> forms);
        public List<string> GetAvailableTeams(List<Form> forms);
        public List<string> GetAvailableWorkprojects(List<Form> forms);
        public List<Permissions> GetAvailablePermissions(List<Form> forms, long userId, IEnumerable<GlobalAccess> globalAccesses);
    }
}
