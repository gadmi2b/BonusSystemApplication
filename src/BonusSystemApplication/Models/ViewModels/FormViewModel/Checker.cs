namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    public static class Checker
    {
        public static bool IsSignaturePossible(Form form, string signatureCheckboxId)
        {
            // no check of situation with signing/dropping already signed/dropped positions

            if(form.IsObjectivesFreezed)
            {
                if (!form.IsResultsFreezed)
                {
                    // objectives signature is allowed only
                    return IsObjectivesAffected(signatureCheckboxId);
                }
                else
                {
                    // results signature is allowed only
                    return IsResultsAffected(signatureCheckboxId);
                }
            }
            return false;
        }
        private static bool IsObjectivesAffected(string signatureCheckboxId)
        {
            switch (signatureCheckboxId)
            {
                // Objectives signature process
                case nameof(Models.Form.IsObjectivesSignedByEmployee):
                case nameof(Models.Form.IsObjectivesSignedByManager):
                case nameof(Models.Form.IsObjectivesSignedByApprover):
                case nameof(Models.Form.IsObjectivesRejectedByEmployee):
                    return true;
            }
            return false;
        }
        private static bool IsResultsAffected(string signatureCheckboxId)
        {
            switch (signatureCheckboxId)
            {
                // Results signature process
                case nameof(Models.Form.IsResultsSignedByEmployee):
                case nameof(Models.Form.IsResultsSignedByManager):
                case nameof(Models.Form.IsResultsSignedByApprover):
                case nameof(Models.Form.IsResultsRejectedByEmployee):
                    return true;
            }
            return false;
        }
    }
}
