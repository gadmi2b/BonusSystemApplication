namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    /// <summary>
    /// Stage #1 of form defenition: form header, participants and objectives 
    /// </summary>
    public class ObjectivesDefinition
    {
        public long FormId { get; set; }
        public bool IsObjectivesFreezed { get; set; } = false;
        public string Period { get; set; }
        public string Year { get; set; }
        public IList<ObjectiveResult> ObjectivesResults { get; set; }
        public string EmployeeFullName { get; set; }
        public string ManagerFullName { get; set; }
        public string ApproverFullName { get; set; }
        public string WorkprojectName { get; set; }
        public bool IsWpmHox { get; set; }

        //readonly values
        public string Team { get; set; }
        public string Position { get; set; }
        public string Pid { get; set; }
        public string WorkprojectDescription { get; set; }
    }
}
