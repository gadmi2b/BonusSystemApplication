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
        public string EmployeeFullName { get; set; } = string.Empty;
        public string ManagerFullName { get; set; } = string.Empty;
        public string ApproverFullName { get; set; } = string.Empty;
        public string WorkprojectName { get; set; } = string.Empty;
        public bool IsWpmHox { get; set; } = false;
        public IList<ObjectiveResult> ObjectivesResults { get; set; }

        //readonly values
        public string TeamName { get; set; } = string.Empty;
        public string PositionName { get; set; } = string.Empty;
        public string Pid { get; set; } = string.Empty;
        public string WorkprojectDescription { get; set; } = string.Empty;

        public ObjectivesDefinition(Form form)
        {
            FormId = form.Id;
            IsObjectivesFreezed = form.IsObjectivesFreezed;
            Period = form.Period.ToString();
            Year = form.Year.ToString();
            EmployeeFullName = $"{form.Employee.LastNameEng} {form.Employee.FirstNameEng}";
            ManagerFullName = $"{form.Manager?.LastNameEng} {form.Manager?.FirstNameEng}";
            ApproverFullName = $"{form.Approver?.LastNameEng} {form.Approver?.FirstNameEng}";
            WorkprojectName = form.Workproject.Name;
            IsWpmHox = form.IsWpmHox;
            TeamName = form.Employee.Team.Name;
            PositionName = form.Employee.Position.NameEng;
            Pid = form.Employee.Pid;
            WorkprojectDescription = form.Workproject.Name;
            ObjectivesResults = form.ObjectivesResults
                .Select(x => new ObjectiveResult
                {
                    Id = x.Id,
                    Row = x.Row,
                    Statement = x.Statement,
                    Description = x.Description,
                    IsKey = x.IsKey,
                    IsMeasurable = x.IsMeasurable,
                    Unit = x.Unit,
                    Threshold = x.Threshold,
                    Target = x.Target,
                    Challenge = x.Challenge,
                    WeightFactor = x.WeightFactor,
                    KpiUpperLimit = x.KpiUpperLimit,
                })
                .ToList();
        }
    }
}
