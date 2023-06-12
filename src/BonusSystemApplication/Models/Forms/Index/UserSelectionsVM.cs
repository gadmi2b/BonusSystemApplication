namespace BonusSystemApplication.Models.Forms.Index
{
    public class UserSelectionsVM
    {
        public List<string> SelectedEmployees { get; set; } = new List<string>();
        public List<string> SelectedPeriods { get; set; } = new List<string>();
        public List<string> SelectedYears { get; set; } = new List<string>();
        public List<string> SelectedPermissions { get; set; } = new List<string>();
        public List<string> SelectedDepartments { get; set; } = new List<string>();
        public List<string> SelectedTeams { get; set; } = new List<string>();
        public List<string> SelectedWorkprojects { get; set; } = new List<string>();
    }
}
