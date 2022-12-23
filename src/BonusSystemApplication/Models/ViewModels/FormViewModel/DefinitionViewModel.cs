namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    public class DefinitionViewModel
    {
        public long Id { get; set; }
        public Periods Period { get; set; }
        public int Year { get; set; }
        public bool IsWpmHox { get; set; }
        public long EmployeeId { get; set; }
        public long? ManagerId { get; set; }
        public long? ApproverId { get; set; }
        public long? WorkprojectId { get; set; }

        public string TeamName { get; set; }
        public string PositionName { get; set; }
        public string Pid { get; set; }
        public string WorkprojectDescription { get; set; }

        public DefinitionViewModel(Definition source)
        {
            Id = source.Id;
            Period = source.Period;
            Year = source.Year;
            IsWpmHox = source.IsWpmHox;
            EmployeeId = source.EmployeeId;
            ManagerId = source.ManagerId;
            ApproverId = source.ApproverId;
            WorkprojectId = source.WorkprojectId;

            TeamName = source.Employee?.Team?.Name == null ? string.Empty : source.Employee.Team.Name;
            PositionName = source.Employee?.Position?.NameEng == null ? string.Empty : source.Employee.Position.NameEng;
            Pid = source.Employee?.Pid == null ? string.Empty : source.Employee.Pid;
            WorkprojectDescription = source.Workproject?.Description == null ? string.Empty : source.Workproject.Description;
        }
    }
}
