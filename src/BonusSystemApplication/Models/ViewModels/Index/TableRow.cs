namespace BonusSystemApplication.Models.ViewModels.Index
{
    public class TableRow
    {
        public long Id { get; set; }
        public string WorkprojectName { get; set; }
        public string EmployeeFullName { get; set; }
        public string DepartmentName { get; set; }
        public string TeamName { get; set; }
        public int Year { get; set; }
        public Periods Period { get; set; }
        public DateTime? LastSavedDateTime { get; set; }
        public List<Permissions> Permissions { get; set; } = new List<Permissions>();
    }
}
