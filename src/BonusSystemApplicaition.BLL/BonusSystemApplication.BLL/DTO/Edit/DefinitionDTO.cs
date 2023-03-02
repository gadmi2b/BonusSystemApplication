using BonusSystemApplication.DAL.Entities;

namespace BonusSystemApplication.BLL.DTO.Edit
{
    public class DefinitionDTO
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

        //public DefinitionDTO(Definition source)
        //{
        //    Id = source.Id;

        //    Period = source.Period.ToString();
        //    Year = source.Year;
        //    IsWpmHox = source.IsWpmHox;
        //    EmployeeId = source.EmployeeId;
        //    ManagerId = source.ManagerId == null ? default : (long)source.ManagerId;
        //    ApproverId = source.ApproverId == null ? default : (long)source.ApproverId;
        //    WorkprojectId = source.WorkprojectId == null ? default : (long)source.WorkprojectId;

        //    TeamName = source.Employee?.Team?.Name == null ? string.Empty : source.Employee.Team.Name;
        //    PositionName = source.Employee?.Position?.NameEng == null ? string.Empty : source.Employee.Position.NameEng;
        //    Pid = source.Employee?.Pid == null ? string.Empty : source.Employee.Pid;
        //    WorkprojectDescription = source.Workproject?.Description == null ? string.Empty : source.Workproject.Description;
        //}
    }
}
