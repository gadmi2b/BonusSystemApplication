using BonusSystemApplication.BLL.DTO.Edit;

namespace BonusSystemApplication.Models.Forms.Edit
{
    public class DefinitionVM
    {
        public long Id { get; set; }
        public string Period { get; set; }
        public int Year { get; set; } = DateTime.Now.Year;
        public bool IsWpmHox { get; set; }
        public long EmployeeId { get; set; }
        public long ManagerId { get; set; }
        public long ApproverId { get; set; }
        public long WorkprojectId { get; set; }

        public string TeamName { get; set; } = string.Empty;
        public string PositionName { get; set; } = string.Empty;
        public string Pid { get; set; } = string.Empty;
        public string WorkprojectDescription { get; set; } = string.Empty;

        public DefinitionVM() { }
        public DefinitionVM(DefinitionDTO source)
        {
            Id = source.Id;

            Period = source.Period;
            Year = source.Year;
            IsWpmHox = source.IsWpmHox;
            EmployeeId = source.EmployeeId;
            ManagerId = source.ManagerId;
            ApproverId = source.ApproverId;
            WorkprojectId = source.WorkprojectId;

            TeamName = source.TeamName;
            PositionName = source.PositionName;
            Pid = source.Pid;
            WorkprojectDescription = source.WorkprojectDescription;
        }
    }
}
