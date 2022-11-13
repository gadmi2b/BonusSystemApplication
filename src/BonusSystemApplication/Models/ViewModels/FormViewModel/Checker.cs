namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    public static class Checker
    {
        public static bool IsSignaturePossible(Form form, string signatureCheckboxId, IPropertyLinker propertyLinker)
        {
            // signing/dropping already signed/dropped positions will not be checked

            // objectives signing is allowed only
            if (form.IsObjectivesFreezed && !form.IsResultsFreezed)
            {
                if (propertyLinker.GetType() == typeof(ObjectivesSignaturePropertyLinker))
                    return IsPropertyAffected(signatureCheckboxId, propertyLinker);
            }

            // results signing is allowed only
            if (form.IsObjectivesFreezed && form.IsResultsFreezed)
            {
                if (propertyLinker.GetType() == typeof(ResultsSignaturePropertyLinker))
                    return IsPropertyAffected(signatureCheckboxId, propertyLinker);
            }

            return false;
        }
        private static bool IsPropertyAffected(string signatureCheckboxId, IPropertyLinker propertyLinker)
        {
            if (propertyLinker.IsSignedIsRejectedPairs.Keys.Contains(signatureCheckboxId) ||
                propertyLinker.IsSignedIsRejectedPairs.Values.Contains(signatureCheckboxId))
            {
                return true;
            }
            return false;
        }

        public static List<Dictionary<string, object>> GetFormPropertiesValuesToSet(Form form,
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
            List<Dictionary<string, object>> formPropsNamesValues = new List<Dictionary<string, object>>();
            Dictionary<string, object> formPropNameValue = new Dictionary<string, object>();

            // if signatureCheckboxId in IsSignedIsRejectedPairs in Keys:
            // => logic applied for all SignedBy properties: sign/drop with signature
            // if for this Key exists Value and
            // if(!isSignatureCheckboxChecked) => we have to drop IsRejected property also

            // if signatureCheckboxId in IsSignedIsRejectedPairs in Values:
            // => logic applied for all SignedBy properties: sign/drop, but without signature

            //if (ObjectivesIsSignedIsRejectedPropNamesPairs.Keys.Contains(signatureCheckboxId) ||
            //    ResultsIsSignedIsRejectedPropNamesPairs.Keys.Contains(signatureCheckboxId))
            //{
            //    formPropNameValue.Add(signatureCheckboxId, isSignatureCheckboxChecked);
            //}


            return formPropsNamesValues;

        }
    }
}
