namespace BonusSystemApplication.BLL.DTO.Edit
{
    public class DefinitionDTO
    {
        public long Id { get; set; }
        public string Period { get; set; } = string.Empty;
        public int Year { get; set; }
        public bool IsWpmHox { get; set; }
        public long EmployeeId { get; set; }
        public long ManagerId { get; set; }
        public long ApproverId { get; set; }
        public long WorkprojectId { get; set; }

        public string TeamName { get; set; } = string.Empty;
        public string PositionName { get; set; } = string.Empty;
        public string Pid { get; set; } = string.Empty;
        public string WorkprojectDescription { get; set; } = string.Empty;
    }
}
