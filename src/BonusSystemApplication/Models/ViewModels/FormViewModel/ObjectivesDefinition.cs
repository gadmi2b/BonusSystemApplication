namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    /// <summary>
    /// Stage #1 of form definition: form header, participants and objectives 
    /// </summary>
    public class ObjectivesDefinition
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
        public IList<Objective> Objectives { get; set; }

        //readonly values
        public string TeamName { get; set; } = string.Empty;
        public string PositionName { get; set; } = string.Empty;
        public string Pid { get; set; } = string.Empty;
        public string WorkprojectDescription { get; set; } = string.Empty;

        public ObjectivesDefinition() { }
        public ObjectivesDefinition(Form form)
        {
            FormId = form.Id;
            IsObjectivesFreezed = form.IsObjectivesFreezed;
            Period = form.Period.ToString();
            Year = form.Year.ToString();
            EmployeeId = form.EmployeeId;
            ManagerId = form.ManagerId == null ? 0 : (long)form.ManagerId;
            ApproverId = form.ApproverId == null ? 0 : (long)form.ApproverId;
            WorkprojectId = form.WorkprojectId == null ? 0 : (long)form.WorkprojectId;
            IsWpmHox = form.IsWpmHox;
            TeamName = form.Employee.Team?.Name == null ? string.Empty : form.Employee.Team.Name;
            PositionName = form.Employee.Position?.NameEng == null ? string.Empty : form.Employee.Position.NameEng;
            Pid = form.Employee.Pid;
            WorkprojectDescription = form.Workproject?.Description == null ? string.Empty : form.Workproject.Description;
            Objectives = form.ObjectivesResults
                .Select(x => new Objective
                {
                    Id = x.Id,
                    Row = x.Row,
                    Statement = x.Statement == null ? string.Empty : x.Statement,
                    Description = x.Description == null ? string.Empty : x.Description,
                    IsKey = x.IsKey,
                    IsMeasurable = x.IsMeasurable,
                    Unit = x.Unit == null ? string.Empty : x.Unit,
                    Threshold = x.Threshold == null ? string.Empty : x.Threshold,
                    Target = x.Target == null ? string.Empty : x.Target,
                    Challenge = x.Challenge == null ? string.Empty : x.Challenge,
                    WeightFactor = x.WeightFactor == null ? string.Empty : x.WeightFactor,
                    KpiUpperLimit = x.KpiUpperLimit == null ? string.Empty : x.KpiUpperLimit,
                })
                .ToList();
        }
    }
}
