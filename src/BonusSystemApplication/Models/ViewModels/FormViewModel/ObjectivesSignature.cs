namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    /// <summary>
    /// Stage #2 of form defenition: objectives were freezed and signatures collection is in progress 
    /// </summary>
    public class ObjectivesSignature
    {
        public string ObjectivesEmployeeSignature { get; set; } = string.Empty;
        public bool IsObjectivesSignedByEmployee { get; set; } = false;
        public bool IsObjectivesRejectedByEmployee { get; set; } = false;
        public string ObjectivesManagerSignature { get; set; } = string.Empty;
        public bool IsObjectivesSignedByManager { get; set; } = false;
        public string ObjectivesApproverSignature { get; set; } = string.Empty;
        public bool IsObjectivesSignedByApprover { get; set; } = false;

        public bool IsObjectivesSigned
        {
            get
            {
                return IsObjectivesSignedByEmployee &&
                       IsObjectivesSignedByManager &&
                       IsObjectivesSignedByApprover;
            }
        }
    }
}
