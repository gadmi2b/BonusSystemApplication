namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    public static class PropertyLinkerHandler
    {
        public static IPropertyLinker AffectedPropertyLinker { get; set; } = null;

        public static bool IsPropertyLinkerAffected(IPropertyLinker propertyLinker,
                                                             string signatureCheckboxId)
        {
            if(propertyLinker == null || string.IsNullOrEmpty(signatureCheckboxId))
            {
                return false;
            }

            if (propertyLinker.IsSignedIsRejectedPairs.Keys.Contains(signatureCheckboxId) ||
                propertyLinker.IsSignedIsRejectedPairs.Values.Contains(signatureCheckboxId))
            {
                AffectedPropertyLinker = propertyLinker;
                return true;
            }
            return false;
        }

        public static List<Dictionary<string, object?>> GetPropertiesValues(string signatureCheckboxId,
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

            List<Dictionary<string, object?>> propertiesValuesToSet = new List<Dictionary<string, object?>>();
            if(AffectedPropertyLinker == null || string.IsNullOrEmpty(signatureCheckboxId))
            {
                return propertiesValuesToSet;
            }

            // TODO: write code here
            Dictionary<string, object?> propertyValue = new Dictionary<string, object?>();


            return propertiesValuesToSet;

        }
        //private bool IsSignatureImpossible(Form form)
        //{
        //    // signing/dropping already signed/dropped positions will not be checked

        //    // objectives signing is allowed only
        //    if (form.IsObjectivesFreezed && !form.IsResultsFreezed)
        //    {
        //        if (AffectedPropertyLinker!.GetType() == typeof(ObjectivesSignaturePropertyLinker))
        //            return true;
        //    }

        //    // results signing is allowed only
        //    if (form.IsObjectivesFreezed && form.IsResultsFreezed)
        //    {
        //        if (AffectedPropertyLinker!.GetType() == typeof(ResultsSignaturePropertyLinker))
        //            return true;
        //    }

        //    return false;
        //}
    }
}
