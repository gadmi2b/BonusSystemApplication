namespace BonusSystemApplication.BLL.DTO.Index
{
    public class TableRowDTO
    {
        public long Id { get; set; }
        public string WorkprojectName { get; set; } = string.Empty;
        public string EmployeeFullName { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public string TeamName { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public string Period { get; set; } = string.Empty;
        public List<string> Permissions { get; set; } = new List<string>();
        public DateTime? LastSavedAt { get; set; }
    }
}
