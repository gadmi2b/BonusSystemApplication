namespace BonusSystemApplication.Models.ViewModels.IndexViewModel
{
    public class TableRow
    {
        public long Id { get; set; }
        public string? WorkprojectName { get; set; }
        public string? EmployeeFullName { get; set; }
        public string? DepartmentName { get; set; }
        public string? TeamName { get; set; }
        public string Year { get; set; }
        public string Period { get; set; }
        public List<string> Permissions { get; set; } = new List<string>();
        public DateTime? LastSavedDateTime { get; set; }

        public bool IsChecked { get; set; } = false;
    }
}
