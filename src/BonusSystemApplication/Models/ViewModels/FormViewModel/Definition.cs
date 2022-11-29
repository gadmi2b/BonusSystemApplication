namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    /// <summary>
    /// Stage #1 of form definition: form header, participants and objectives 
    /// </summary>
    public class Definition
    {
        public long FormId { get; set; } = 0;
        public bool IsObjectivesFreezed { get; set; } = false;
        public string Period { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public long EmployeeId { get; set; } = 0;
        public long ManagerId { get; set; } = 0;
        public long ApproverId { get; set; } = 0;
        public long WorkprojectId { get; set; } = 0;
        public bool IsWpmHox { get; set; } = false;

        //readonly values
        public string TeamName { get; set; } = string.Empty;
        public string PositionName { get; set; } = string.Empty;
        public string Pid { get; set; } = string.Empty;
        public string WorkprojectDescription { get; set; } = string.Empty;

        public Definition() { }
        public Definition(Form form)
        {
            FormId = form.Id;
            IsObjectivesFreezed = form.IsObjectivesFreezed;
            Period = form.Period.ToString();
            Year = form.Year.ToString();
            EmployeeId = form.Employee?.Id == null ? 0 : form.Employee.Id;
            ManagerId = form.Manager?.Id == null ? 0 : form.Manager.Id;
            ApproverId = form.Approver?.Id == null ? 0 : form.Approver.Id;
            WorkprojectId = form.Workproject?.Id == null ? 0 : form.Workproject.Id;
            IsWpmHox = form.IsWpmHox;
            TeamName = form.Employee?.Team?.Name == null ? string.Empty : form.Employee.Team.Name;
            PositionName = form.Employee?.Position?.NameEng == null ? string.Empty : form.Employee.Position.NameEng;
            Pid = form.Employee?.Pid == null ? string.Empty : form.Employee.Pid;
            WorkprojectDescription = form.Workproject?.Description == null ? string.Empty : form.Workproject.Description;
        }
    }
}
