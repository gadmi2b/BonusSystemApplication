namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    public static class Checker
    {
        public static Dictionary<string, string?> ObjectivesIsSignedIsRejectedPropNamesPairs = new Dictionary<string, string?>()
            {
                { nameof(Form.IsObjectivesSignedByEmployee), nameof(Form.IsObjectivesRejectedByEmployee)},
                { nameof(Form.IsObjectivesSignedByManager), null},
                { nameof(Form.IsObjectivesSignedByApprover), null},
            };
        public static Dictionary<string, string?> ResultsIsSignedIsRejectedPropNamesPairs = new Dictionary<string, string?>()
            {
                { nameof(Form.IsResultsSignedByEmployee), nameof(Form.IsResultsRejectedByEmployee) },
                { nameof(Form.IsResultsSignedByManager), null },
                { nameof(Form.IsResultsSignedByApprover), null },
            };

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
            if (ObjectivesIsSignedIsRejectedPropNamesPairs.Keys.Contains(signatureCheckboxId) ||
                ObjectivesIsSignedIsRejectedPropNamesPairs.Values.Contains(signatureCheckboxId))
            {
                return true;
            }
            return false;
        }
        private static bool IsResultsAffected(string signatureCheckboxId)
        {
            if (ResultsIsSignedIsRejectedPropNamesPairs.Keys.Contains(signatureCheckboxId) ||
                ResultsIsSignedIsRejectedPropNamesPairs.Values.Contains(signatureCheckboxId))
            {
                return true;
            }

            return false;
        }

        public static Dictionary<string, object> SignatureClassificator(Form form,
                                                  string signatureCheckboxId,
                                                  bool isSignatureCheckboxChecked)
        {
            /*   Signed / Rejected pairs:
             *   IsObjectivesSignedByEmployee: if(!isSignatureCheckboxChecked) { IsObjectivesSignedByEmployee = false and 
             *                                                                 ObjectivesEmployeeSignature = string.Empty
             *                                                                 IsObjectivesRejectedByEmployee = false }
             *   IsObjectivesRejectedByEmployee: if(!isSignatureCheckboxChecked) { IsObjectivesSignedByEmployee = false and 
             *                                                                 ObjectivesEmployeeSignature = string.Empty
             *                                                                 IsObjectivesRejectedByEmployee = false }                                                              
             *   Other variants:
             *   IsObjectivesSignedByXXX: if(isSignatureCheckboxChecked)  { IsObjectivesSignedByXXX = true and 
             *                                                              ObjectivesXXXSignature = signature }
            */
            Dictionary<string, object> formPropNameValue = new Dictionary<string, object>();

            // if signatureCheckboxId in IsSignedIsRejectedPairs in Keys:
            // => logic applied for all SignedBy properties: sign/drop with signature
            // if for this Key exists Value and
            // if(!isSignatureCheckboxChecked) => we have to drop IsRejected property also

            // if signatureCheckboxId in IsSignedIsRejectedPairs in Values:
            // => logic applied for all SignedBy properties: sign/drop, but without signature


            return formPropNameValue;

        }
    }
}
