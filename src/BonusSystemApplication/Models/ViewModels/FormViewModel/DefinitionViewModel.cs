using BonusSystemApplication.DAL.Entities;

namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    public class DefinitionViewModel
    {
        public Periods Period { get; set; }
        public int Year { get; set; } = DateTime.Now.Year;
        public bool IsWpmHox { get; set; }
        public long EmployeeId { get; set; }
        public long? ManagerId { get; set; }
        public long? ApproverId { get; set; }
        public long? WorkprojectId { get; set; }

        public string? TeamName { get; set; }
        public string? PositionName { get; set; }
        public string? Pid { get; set; }
        public string? WorkprojectDescription { get; set; }

        public DefinitionViewModel() { }
        public DefinitionViewModel(Definition source)
        {
            Period = source.Period;
            Year = source.Year;
            IsWpmHox = source.IsWpmHox;
            EmployeeId = source.EmployeeId;
            ManagerId = source.ManagerId;
            ApproverId = source.ApproverId;
            WorkprojectId = source.WorkprojectId;

            TeamName = source.Employee?.Team?.Name;
            PositionName = source.Employee?.Position?.NameEng;
            Pid = source.Employee?.Pid;
            WorkprojectDescription = source.Workproject?.Description;
        }
    }
}
