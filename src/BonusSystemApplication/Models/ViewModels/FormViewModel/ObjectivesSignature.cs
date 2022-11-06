namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    /// <summary>
    /// Stage #2 of form definition: objectives were freezed and signatures collection is in progress 
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

        public ObjectivesSignature() { }
        public ObjectivesSignature(Form form)
        {
            IsObjectivesSignedByEmployee = form.IsObjectivesSignedByEmployee;
            ObjectivesEmployeeSignature = form.ObjectivesEmployeeSignature == null ? string.Empty : form.ObjectivesEmployeeSignature;
            IsObjectivesRejectedByEmployee = form.IsObjectivesRejectedByEmployee;
            IsObjectivesSignedByManager = form.IsObjectivesSignedByManager;
            ObjectivesManagerSignature = form.ObjectivesManagerSignature == null ? string.Empty : form.ObjectivesManagerSignature;
            IsObjectivesSignedByApprover = form.IsObjectivesSignedByApprover;
            ObjectivesApproverSignature = form.ObjectivesApproverSignature == null ? string.Empty : form.ObjectivesApproverSignature;

            // Initial varian based on separated loading logic
            //if (form.IsObjectivesFreezed)
            //{
            //  IsObjectivesSignedByEmployee = form.IsObjectivesSignedByEmployee;
            //  ObjectivesEmployeeSignature = form.ObjectivesEmployeeSignature == null ? string.Empty : form.ObjectivesEmployeeSignature;
            //  IsObjectivesRejectedByEmployee = form.IsObjectivesRejectedByEmployee;
            //  IsObjectivesSignedByManager = form.IsObjectivesSignedByManager;
            //  ObjectivesManagerSignature = form.ObjectivesManagerSignature == null ? string.Empty : form.ObjectivesManagerSignature;
            //  IsObjectivesSignedByApprover = form.IsObjectivesSignedByApprover;
            //  ObjectivesApproverSignature = form.ObjectivesApproverSignature == null ? string.Empty : form.ObjectivesApproverSignature;
            //}
            //else
            //{
            //  do nothing
            //}
        }
    }
}
